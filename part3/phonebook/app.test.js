// @vitest-environment node
import { beforeAll, beforeEach, afterAll, describe, expect, it } from 'vitest'
import mongoose from 'mongoose'
import request from 'supertest'
import app from './app.js'
import Person from './models/person.js'
import { connectDatabase } from './database.js'
import { execFile } from 'node:child_process'
import { promisify } from 'node:util'
import path from 'node:path'

const execute = promisify(execFile)
beforeAll(async () => {
  const database = new URL(process.env.MONGODB_URI).pathname.slice(1)
  if (!database.endsWith('_test')) throw new Error('Tests require a dedicated database ending in _test')
  await connectDatabase(); await Person.init()
}, 20000)
beforeEach(async () => { await Person.deleteMany({}) })
afterAll(async () => { await mongoose.connection.close() })

describe('phonebook HTTP and database integration', () => {
  it('runs the command-line add and list operations against MongoDB', async () => {
    const script = path.join(import.meta.dirname, 'mongo.js')
    const options = { cwd: import.meta.dirname, env: process.env, timeout: 20000 }
    const added = await execute(process.execPath, [script, 'env-auth', 'CLI Fixture', '040-1234567'], options)
    expect(added.stdout).toContain('added CLI Fixture number 040-1234567 to phonebook')
    const listed = await execute(process.execPath, [script, 'env-auth'], options)
    expect(listed.stdout).toContain('CLI Fixture 040-1234567')
    expect(await Person.countDocuments({ name: 'CLI Fixture' })).toBe(1)
  }, 25000)

  it('persists a record, lists it, reads it, updates it and deletes it', async () => {
    const created = await request(app).post('/api/persons').send({ name: 'Sample Person', number: '040-1234567' }).expect(201)
    expect(created.body.id).toMatch(/^[a-f\d]{24}$/)
    expect(created.body._id).toBeUndefined()
    expect(created.body.nameKey).toBeUndefined()
    expect(await Person.countDocuments({})).toBe(1)
    const listed = await request(app).get('/api/persons').expect(200)
    expect(listed.body).toEqual([created.body])
    const id = created.body.id
    await request(app).get(`/api/persons/${id}`).expect(200)
    const updated = await request(app).put(`/api/persons/${id}`).send({ name: 'Sample Person', number: '09-7654321' }).expect(200)
    expect(updated.body.number).toBe('09-7654321')
    expect((await Person.findById(id)).number).toBe('09-7654321')
    await request(app).delete(`/api/persons/${id}`).expect(204)
    await request(app).get(`/api/persons/${id}`).expect(404)
    expect(await Person.countDocuments({})).toBe(0)
  })

  it('rejects missing fields, short names, invalid numbers and duplicate names', async () => {
    await request(app).post('/api/persons').send({ name: 'Ab' }).expect(400)
    await request(app).post('/api/persons').send({ name: 'Ab', number: '040-1234567' }).expect(400)
    for (const number of ['1234556', '1-22334455', '10-22-334455', '09-123']) {
      const result = await request(app).post('/api/persons').send({ name: 'Sample Person', number }).expect(400)
      expect(result.body.error).toBeTruthy()
    }
    await request(app).post('/api/persons').send({ name: 'Sample Person', number: '040-1234567' }).expect(201)
    const duplicate = await request(app).post('/api/persons').send({ name: 'sample person', number: '09-1234567' }).expect(400)
    expect(duplicate.body.error).toBe('name must be unique')
    expect(await Person.countDocuments({})).toBe(1)
  })

  it('validates updates and leaves the old value intact on failure', async () => {
    const person = await new Person({ name: 'Sample Person', number: '040-1234567' }).save()
    await request(app).put(`/api/persons/${person.id}`).send({ name: 'Sample Person', number: 'invalid' }).expect(400)
    expect((await Person.findById(person.id)).number).toBe('040-1234567')
  })

  it('distinguishes malformed IDs from missing records and validates JSON', async () => {
    await request(app).get('/api/persons/not-an-id').expect(400)
    const missing = new mongoose.Types.ObjectId().toString()
    await request(app).get(`/api/persons/${missing}`).expect(404)
    await request(app).delete(`/api/persons/${missing}`).expect(404)
    await request(app).post('/api/persons').set('Content-Type', 'application/json').send('{').expect(400)
    await request(app).get('/api/unknown').expect(404)
  })

  it('reports the live database count and serves the built frontend', async () => {
    await new Person({ name: 'Sample Person', number: '040-1234567' }).save()
    const info = await request(app).get('/info').expect(200)
    expect(info.text).toContain('1 people')
    const page = await request(app).get('/').expect(200)
    expect(page.text).toContain('<div id="root"></div>')
    expect(page.text).toContain('/assets/')
  })
})
