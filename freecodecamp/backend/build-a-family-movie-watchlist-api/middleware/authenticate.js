import jwt from 'jsonwebtoken';
export function authenticate(req, res, next) {
  const authorization = req.headers.authorization;
  const match = typeof authorization === 'string' && authorization.match(/^Bearer\s+(\S+)$/i);
  if (!match) return res.status(401).json({ error: 'No token provided.' });
  try {
    const decoded = jwt.verify(match[1], process.env.JWT_SECRET, { algorithms: ['HS256'] });
    if (!decoded || typeof decoded !== 'object' || !Number.isInteger(decoded.id)) throw new Error('Invalid claims');
    req.user = decoded;
    return next();
  } catch {
    return res.status(401).json({ error: 'Invalid or expired token.' });
  }
}
