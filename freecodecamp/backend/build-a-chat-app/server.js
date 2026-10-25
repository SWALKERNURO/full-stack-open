import http from 'http';
import fs from 'fs';
import { WebSocket, WebSocketServer } from 'ws';

const PORT = 3001;
const publicDirectory = new URL('./public/', import.meta.url);
const server = http.createServer((req, res) => {
  let pathname;
  try { pathname = new URL(req.url, 'http://localhost').pathname; }
  catch { res.writeHead(400); res.end('Bad request'); return; }
  const asset = pathname === '/' ? ['index.html', 'text/html; charset=utf-8']
    : pathname === '/script.js' ? ['script.js', 'text/javascript; charset=utf-8'] : null;
  if (!asset) { res.writeHead(404); res.end('Not found'); return; }
  if (req.method !== 'GET' && req.method !== 'HEAD') {
    res.writeHead(405, { Allow: 'GET, HEAD' }); res.end(); return;
  }
  fs.readFile(new URL(asset[0], publicDirectory), (error, content) => {
    if (error) { res.writeHead(500); res.end('Unable to load page'); return; }
    res.writeHead(200, { 'Content-Type': asset[1], 'X-Content-Type-Options': 'nosniff' });
    res.end(req.method === 'HEAD' ? undefined : content);
  });
});
const wss = new WebSocketServer({ server, maxPayload: 4096 });
function broadcast(payload) {
  const message = JSON.stringify(payload);
  wss.clients.forEach(client => {
    if (client.readyState === WebSocket.OPEN) client.send(message);
  });
}
wss.on('connection', (socket, req) => {
  const username = (new URL(req.url, 'http://localhost').searchParams.get('username') || '').trim().slice(0, 20) || 'Anonymous';
  broadcast({ type: 'system', text: username + ' joined' });
  socket.on('message', (raw, isBinary) => {
    if (isBinary) return;
    let message;
    try { message = JSON.parse(raw.toString()); } catch { return; }
    if (!message || typeof message.text !== 'string') return;
    const text = message.text.trim();
    if (!text || text.length > 2000) return;
    broadcast({ type: 'chat', username, text });
  });
  socket.on('close', () => broadcast({ type: 'system', text: username + ' left' }));
  socket.on('error', () => {});
});
server.listen(PORT, () => console.log('Chat server running at http://localhost:3001'));
