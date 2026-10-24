import express from 'express'
import cors from 'cors'
import blogs from './controllers/blogs.js'
import users from './controllers/users.js'
import login from './controllers/login.js'
import testing from './controllers/testing.js'
import { tokenExtractor, unknownEndpoint, errorHandler } from './utils/middleware.js'

const app = express()
app.use(cors())
app.use(express.json({ limit: '16kb' }))
app.use(tokenExtractor)
app.get('/health', (_request, response) => response.json({ status: 'ok' }))
app.use('/api/blogs', blogs)
app.use('/api/users', users)
app.use('/api/login', login)
if (process.env.NODE_ENV === 'test') app.use('/api/testing', testing)
app.use(unknownEndpoint)
app.use(errorHandler)
export default app
