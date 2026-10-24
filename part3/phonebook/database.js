import mongoose from 'mongoose'

let connectionPromise

export const connectDatabase = async () => {
  if (mongoose.connection.readyState === 1) return mongoose.connection
  if (!connectionPromise) {
    if (!process.env.MONGODB_URI) {
      const error = new Error('Database is not configured')
      error.status = 503
      throw error
    }
    connectionPromise = mongoose.connect(process.env.MONGODB_URI, { serverSelectionTimeoutMS: 10000 })
      .catch(error => { connectionPromise = undefined; throw error })
  }
  return connectionPromise
}
