import express from 'express';
import helmet from 'helmet';
import authRoutes from './routes/auth.js';
import watchlistRoutes from './routes/watchlist.js';

if (!process.env.JWT_SECRET) throw new Error('Set JWT_SECRET before starting the watchlist API.');
const PORT = Number(process.env.PORT || 3000);
const app = express();
app.disable('x-powered-by');
app.use(helmet());
app.use(express.json({ limit: '16kb' }));
app.get('/', (_req, res) => res.send('Family Movie Watchlist API'));
app.use('/api/auth', authRoutes);
app.use('/api/watchlist', watchlistRoutes);
app.use((_req, res) => res.status(404).json({ error: 'Not found.' }));
app.use((error, _req, res, _next) => {
  if (error instanceof SyntaxError && error.status === 400) return res.status(400).json({ error: 'Invalid JSON.' });
  return res.status(500).json({ error: 'Unable to process request.' });
});
app.listen(PORT, () => console.log('Watchlist server listening on port ' + PORT));
