import app from './app.js'
import { connectDatabase } from './database.js'

const port = Number(process.env.PORT || 3001)
try {
  await connectDatabase()
  app.listen(port, () => console.log(`Phonebook listening on port ${port}`))
} catch (error) {
  console.error('Could not start phonebook:', error.name)
  process.exitCode = 1
}
