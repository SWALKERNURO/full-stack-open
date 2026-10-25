export function authorizeModification(req, res, next) {
  const user = req.user;
  if (user && (user.role === 'parent' ||
      (user.role === 'child' && String(user.id) === req.params.userId))) return next();
  return res.status(403).json({ error: 'Access denied' });
}
