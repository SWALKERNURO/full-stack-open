import 'dotenv/config'
import express from 'express'
import cors from 'cors'
import morgan from 'morgan'
import path from 'node:path'
import Person from './models/person.js'
import { connectDatabase } from './database.js'

const app = express()
app.use(cors(process.env.CORS_ORIGIN ? { origin: process.env.CORS_ORIGIN } : {}))
app.use(express.json({ limit: '16kb' }))
morgan.token('post-body', request => request.method === 'POST' ? JSON.stringify(request.body) : '')
if (process.env.NODE_ENV !== 'test') {
  app.use(morgan(':method :url :status :res[content-length] - :response-time ms :post-body'))
}

app.get('/health', (_request, response) => response.json({ status: 'ok' }))

const requireDatabase = async (_request, _response, next) => {
  try { await connectDatabase(); next() } catch (error) { next(error) }
}
app.use('/api', requireDatabase)
app.use('/info', requireDatabase)

app.get('/info', async (_request, response) => {
  const receivedAt = new Date()
  const count = await Person.countDocuments({})
  response.type('html').send(`<p>Phonebook has info for ${count} people</p><p>${receivedAt.toString()}</p>`)
})

app.get('/api/persons', async (_request, response) => response.json(await Person.find({})))

app.get('/api/persons/:id', async (request, response) => {
  const person = await Person.findById(request.params.id)
  if (!person) return response.status(404).json({ error: 'person not found' })
  response.json(person)
})

app.post('/api/persons', async (request, response) => {
  const { name, number } = request.body
  const person = new Person({ name, number })
  const saved = await person.save()
  response.status(201).json(saved)
})

app.put('/api/persons/:id', async (request, response) => {
  const { name, number } = request.body
  if (typeof name !== 'string' || typeof number !== 'string') {
    return response.status(400).json({ error: 'name and number are required' })
  }
  const person = await Person.findByIdAndUpdate(request.params.id,
    { name: name.trim(), nameKey: name.trim().toLowerCase(), number: number.trim() },
    { new: true, runValidators: true, context: 'query' })
  if (!person) return response.status(404).json({ error: 'person not found' })
  response.json(person)
})

app.delete('/api/persons/:id', async (request, response) => {
  const person = await Person.findByIdAndDelete(request.params.id)
  if (!person) return response.status(404).json({ error: 'person not found' })
  response.status(204).end()
})

app.use('/api', (_request, response) => response.status(404).json({ error: 'unknown endpoint' }))
app.use(express.static(path.join(import.meta.dirname, 'dist')))
app.get('/', (_request, response) => response.status(503).type('text').send('Build the phonebook frontend before serving the application.'))
app.use((_request, response) => response.status(404).json({ error: 'unknown endpoint' }))

app.use((error, _request, response, _next) => {
  if (error.name === 'CastError') return response.status(400).json({ error: 'malformatted id' })
  if (error.name === 'ValidationError') return response.status(400).json({ error: error.message })
  if (error.code === 11000) return response.status(400).json({ error: 'name must be unique' })
  if (error.type === 'entity.parse.failed') return response.status(400).json({ error: 'invalid JSON' })
  if (error.type === 'entity.too.large') return response.status(413).json({ error: 'request too large' })
  if (error.status === 503 || error.name === 'MongooseServerSelectionError') {
    return response.status(503).json({ error: 'database unavailable' })
  }
  console.error('Request failed:', error.name)
  response.status(500).json({ error: 'internal server error' })
})

export default app
