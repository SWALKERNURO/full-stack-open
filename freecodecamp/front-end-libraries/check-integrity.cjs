const { chromium } = require('@playwright/test');
const http = require('http');
const fs = require('fs');
const path = require('path');
const assert = require('assert/strict');
(async () => {
 const root=__dirname;
 // Preload trusted demo files: HTTP request paths never reach the filesystem.
 const files=new Map();
 for(const project of ['currency-converter','digital-pet','tic-tac-toe']) {
   for(const name of ['index.html','index.jsx','index.tsx','styles.css']) {
     const filename=path.join(root,project,name);
     if(fs.existsSync(filename)) files.set(`/${project}/${name}`,{data:fs.readFileSync(filename),type:name.endsWith('.html')?'text/html':name.endsWith('.css')?'text/css':'text/plain'});
   }
 }
 const server=http.createServer((req,res)=>{
   const file=files.get(req.url.split('?')[0]);
   if(!file){res.statusCode=404;res.end();return;}
   res.setHeader('Content-Type',file.type);res.end(file.data);
 }).listen(0,'127.0.0.1');
 await new Promise(r=>server.once('listening',r));
 const browser=await chromium.launch({headless:true});
 try {
 const context=await browser.newContext();
 for(const project of ['currency-converter','digital-pet','tic-tac-toe']) {
   const page=await context.newPage();const errors=[];page.on('pageerror',e=>errors.push(e.message));
   await page.goto(`http://127.0.0.1:${server.address().port}/${project}/index.html`);
   await page.waitForFunction(()=>document.querySelector('#root')?.childElementCount>0,{},{timeout:30000});
   assert.equal(await page.locator('script[integrity][crossorigin="anonymous"]').count(),3);
   assert.deepEqual(errors,[]);console.log(project+': rendered with verified pinned CDN scripts');await page.close();
 }
 const page=await context.newPage();const errors=[];page.on('console',m=>errors.push(m.text()));
 await page.route('https://cdnjs.cloudflare.com/ajax/libs/react/18.3.1/umd/react.development.min.js',r=>r.fulfill({status:200,contentType:'text/javascript',headers:{'access-control-allow-origin':'*'},body:'window.__tampered=true;'}));
 await page.goto(`http://127.0.0.1:${server.address().port}/tic-tac-toe/index.html`);
 assert.equal(await page.evaluate(()=>window.__tampered),undefined);
 assert.ok(errors.some(e=>/integrity|digest/i.test(e)));console.log('tampered CDN script: browser blocked execution');
 } finally {await browser.close();server.close();}
})().catch(e=>{console.error(e);process.exit(1)});
