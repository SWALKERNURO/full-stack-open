import express from 'express';
const app = express();
app.disable('x-powered-by');
app.get('/', (_req, res) => res.send("Welcome to Camper Bot's homepage!"));
app.get('/hobbies', (_req, res) => res.send('I cycle, go boating, and play guitar.'));
app.get('/skills', (_req, res) => res.send('JavaScript, Node.js, and Express.js!'));
app.get('/api/profile', (_req, res) => res.json({
  name: 'Camper Bot',
  hobbies: ['cycling', 'boating', 'guitar'],
  skills: ['JavaScript', 'Node.js', 'Express.js']
}));
const server = app.listen(3000, () => console.log('Profile server listening on port 3000'));
export { app, server };
