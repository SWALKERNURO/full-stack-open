import 'dotenv/config'
import mongoose from 'mongoose'
import Person from './models/person.js'

const [password, name, number] = process.argv.slice(2)
const template = process.env.MONGODB_URI_TEMPLATE
const uri = template && password ? template.replace('<password>', encodeURIComponent(password)) : process.env.MONGODB_URI

if (!password || !uri || Boolean(name) !== Boolean(number)) {
  console.error('Usage: node mongo.js PASSWORD ["NAME" NUMBER]. Configure MONGODB_URI_TEMPLATE or MONGODB_URI first.')
  process.exitCode = 1
} else {
  try {
    await mongoose.connect(uri, { serverSelectionTimeoutMS: 10000 })
    if (name && number) {
      const person = await new Person({ name, number }).save()
      console.log(`added ${person.name} number ${person.number} to phonebook`)
    } else {
      console.log('phonebook:')
      const persons = await Person.find({})
      persons.forEach(person => console.log(`${person.name} ${person.number}`))
    }
  } catch (error) {
    console.error('Database operation failed:', error.name)
    process.exitCode = 1
  } finally { await mongoose.connection.close() }
}
