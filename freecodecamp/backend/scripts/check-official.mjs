import { readFile } from 'node:fs/promises';
import { join, resolve } from 'node:path';
import { pathToFileURL } from 'node:url';
import { createRequire } from 'node:module';
import { spawn } from 'node:child_process';
import { once } from 'node:events';
import net from 'node:net';
import strict from 'node:assert/strict';

const root = resolve(process.argv[2] || process.cwd());
process.env.INIT_CWD = root;
process.env.JWT_SECRET = 'grading-secret-value';
const require = createRequire(join(root, 'package.json'));
const { assert } = await import(pathToFileURL(require.resolve('chai')).href);
const helpers = await import(pathToFileURL(join(root, 'tooling/helpers.js')).href);
const AsyncFunction = Object.getPrototypeOf(async function () {}).constructor;
const projects = [
  ['build-a-prime-number-checker-module', null, null],
  ['build-a-personal-profile-app', 'index.js', 3000],
  ['build-a-timestamp-microservice', 'server.js', 8000],
  ['build-a-chat-app', 'server.js', 3001],
  ['build-a-family-movie-watchlist-api', 'index.js', 3000]
];

function section(markdown, heading) {
  const marker = '### --' + heading + '--';
  const start = markdown.indexOf(marker);
  if (start < 0) return '';
  const rest = markdown.slice(start + marker.length);
  const end = rest.search(/\n### /);
  return end < 0 ? rest : rest.slice(0, end);
}
function blocks(text) {
  return [...text.matchAll(/```js\s*\r?\n([\s\S]*?)```/g)].map(match => match[1]);
}
async function isOpen(port) {
  return new Promise(resolve => {
    const socket = net.createConnection({ host: '127.0.0.1', port });
    socket.once('connect', () => { socket.destroy(); resolve(true); });
    socket.once('error', () => resolve(false));
  });
}
async function waitForPort(port, child) {
  for (let attempt = 0; attempt < 150; attempt++) {
    if (child.exitCode !== null) throw new Error('Server exited before becoming ready.');
    if (await isOpen(port)) return;
    await new Promise(resolve => setTimeout(resolve, 100));
  }
  throw new Error('Server did not open port ' + port);
}
async function stop(child) {
  if (!child || child.exitCode !== null) return;
  const exited = once(child, 'exit');
  child.kill('SIGTERM');
  const timer = setTimeout(() => child.kill('SIGKILL'), 2000);
  try { await exited; } finally { clearTimeout(timer); }
}
function waitForMessage(socket, predicate) {
  return new Promise((resolve, reject) => {
    const cleanup = () => { clearTimeout(timer); socket.removeEventListener('message', onMessage); };
    const onMessage = event => {
      const data = JSON.parse(event.data);
      if (predicate(data)) { cleanup(); resolve(data); }
    };
    const timer = setTimeout(() => { cleanup(); reject(new Error('WebSocket message timed out')); }, 3000);
    socket.addEventListener('message', onMessage);
  });
}
async function extraChecks(name) {
  if (name === 'build-a-timestamp-microservice') {
    strict.equal((await (await fetch('http://127.0.0.1:8000/api/0')).json()).unix, 0);
    strict.equal((await (await fetch('http://127.0.0.1:8000/api/-1000')).json()).unix, -1000);
    strict.equal((await (await fetch('http://127.0.0.1:8000/api/99999999999999999999999')).json()).error, 'Invalid Date');
    const before = Date.now();
    const now = await (await fetch('http://127.0.0.1:8000/api')).json();
    strict.ok(now.unix >= before && now.unix <= Date.now());
    strict.equal(now.utc, new Date(now.unix).toUTCString());
    return 4;
  }
  if (name === 'build-a-chat-app') {
    const script = await fetch('http://127.0.0.1:3001/script.js');
    strict.equal(script.status, 200);
    strict.match(script.headers.get('content-type'), /javascript/);
    strict.match(await script.text(), /location\.host/);
    strict.equal((await fetch('http://127.0.0.1:3001/not-a-file')).status, 404);
    const client = new WebSocket('ws://127.0.0.1:3001?username=ActualName');
    try {
      await once(client, 'open');
      client.send('{invalid json');
      const reply = waitForMessage(client, message => message.type === 'chat');
      client.send(JSON.stringify({ username: 'SpoofedName', text: 'Still connected' }));
      strict.deepEqual(await reply, { type: 'chat', username: 'ActualName', text: 'Still connected' });
    } finally { client.close(); }
    return 3;
  }
  if (name === 'build-a-family-movie-watchlist-api') {
    const base = 'http://127.0.0.1:3000';
    async function request(path, method = 'GET', body, token) {
      return fetch(base + path, {
        method,
        headers: { 'Content-Type': 'application/json', ...(token ? { Authorization: 'Bearer ' + token } : {}) },
        ...(body === undefined ? {} : { body: JSON.stringify(body) })
      });
    }
    strict.equal((await request('/api/auth/login', 'POST', { username: {}, password: [] })).status, 400);
    const login = await (await request('/api/auth/login', 'POST', { username: 'jake_miller', password: 'child123' })).json();
    const token = login.token;
    strict.equal((await request('/api/watchlist/999', 'GET', undefined, token)).status, 404);
    strict.equal((await request('/api/watchlist/3/movies', 'POST', { title: '', genre: 'Drama' }, token)).status, 400);
    const created = await (await request('/api/watchlist/3/movies', 'POST', { title: 'Edge case movie', genre: 'Drama' }, token)).json();
    const update = await (await request('/api/watchlist/3/movies/' + created.id, 'PUT', { title: 'Renamed', id: 999, watched: true }, token)).json();
    strict.equal(update.id, created.id);
    strict.equal(update.title, 'Renamed');
    strict.equal((await request('/api/watchlist/3/movies/' + created.id, 'PUT', { watched: 'true' }, token)).status, 400);
    strict.equal((await request('/api/watchlist/3/movies/' + created.id, 'DELETE', undefined, token)).status, 200);
    const final = await (await request('/api/watchlist/3', 'GET', undefined, token)).json();
    strict.ok(!final.some(movie => movie.id === created.id));
    return 6;
  }
  return 0;
}

let officialPassed = 0;
let extraPassed = 0;
for (const [name, entry, port] of projects) {
  const markdown = await readFile(join(root, 'curriculum/locales/english', name + '.md'), 'utf8');
  const tests = blocks(section(markdown, 'tests'));
  const beforeEach = blocks(section(markdown, 'before-each')).join('\n');
  strict.ok(tests.length, 'No official tests found for ' + name);
  let child;
  let output = '';
  try {
    if (entry) {
      child = spawn(process.execPath, [entry], { cwd: join(root, name), env: { ...process.env, PORT: String(port), TZ: 'UTC' }, stdio: ['ignore', 'pipe', 'pipe'] });
      for (const stream of [child.stdout, child.stderr]) stream.on('data', chunk => { output = (output + chunk).slice(-4000); });
      await waitForPort(port, child);
    }
    for (let index = 0; index < tests.length; index++) {
      const run = new AsyncFunction('assert', '__helpers', 'project', 'ROOT', 'WebSocket', beforeEach + '\n' + tests[index]);
      try { await run(assert, helpers, { dashedName: name }, root, WebSocket); }
      catch (error) { throw new Error(name + ' official check ' + (index + 1) + ': ' + error.message, { cause: error }); }
      officialPassed++;
    }
    extraPassed += await extraChecks(name);
    console.log(name + ': ' + tests.length + ' official checks passed.');
  } catch (error) {
    console.error(output);
    throw error;
  } finally { await stop(child); }
}
console.log('OFFICIAL_CHECKS_PASSED=' + officialPassed);
console.log('ADDITIONAL_CHECKS_PASSED=' + extraPassed);
console.log('Project verification only. No course records or submissions were modified.');
