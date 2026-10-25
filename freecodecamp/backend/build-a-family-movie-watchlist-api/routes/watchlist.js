import { Router } from 'express';
import { authenticate } from '../middleware/authenticate.js';
import { authorizeModification } from '../middleware/authorize.js';
import { findById, getWatchlist, addMovie, updateMovie, deleteMovie } from '../utils/db.js';

const router = Router();
router.use(authenticate);
router.param('userId', (req, res, next, value) => {
  if (!/^[1-9]\d*$/.test(value) || !findById(Number(value))) return res.status(404).json({ error: 'User not found.' });
  next();
});
router.param('movieId', (req, res, next, value) => {
  if (!/^[1-9]\d*$/.test(value)) return res.status(404).json({ error: 'Movie not found.' });
  next();
});
router.get('/:userId', (req, res) => res.json(getWatchlist(Number(req.params.userId))));
router.post('/:userId/movies', authorizeModification, (req, res) => {
  const { title, genre } = req.body || {};
  if (typeof title !== 'string' || !title.trim() || title.length > 200 ||
      typeof genre !== 'string' || !genre.trim() || genre.length > 80) return res.status(400).json({ error: 'A valid title and genre are required.' });
  const movie = addMovie(Number(req.params.userId), { title: title.trim(), genre: genre.trim() });
  return res.status(201).json(movie);
});
router.put('/:userId/movies/:movieId', authorizeModification, (req, res) => {
  const body = req.body || {};
  const updates = {};
  for (const field of ['title', 'genre']) {
    if (Object.hasOwn(body, field)) {
      if (typeof body[field] !== 'string' || !body[field].trim() || body[field].length > (field === 'title' ? 200 : 80)) return res.status(400).json({ error: 'Invalid movie details.' });
      updates[field] = body[field].trim();
    }
  }
  if (Object.hasOwn(body, 'watched')) {
    if (typeof body.watched !== 'boolean') return res.status(400).json({ error: 'watched must be a boolean.' });
    updates.watched = body.watched;
  }
  if (!Object.keys(updates).length) return res.status(400).json({ error: 'No valid fields to update.' });
  const movie = updateMovie(Number(req.params.userId), Number(req.params.movieId), updates);
  if (!movie) return res.status(404).json({ error: 'Movie not found.' });
  return res.json(movie);
});
router.delete('/:userId/movies/:movieId', authorizeModification, (req, res) => {
  const removed = deleteMovie(Number(req.params.userId), Number(req.params.movieId));
  if (!removed) return res.status(404).json({ error: 'Movie not found.' });
  return res.json({ message: 'Movie removed.' });
});
export default router;
