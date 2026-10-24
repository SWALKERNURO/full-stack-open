import mongoose from 'mongoose'

const personSchema = new mongoose.Schema({
  name: { type: String, required: true, trim: true, minlength: 3 },
  nameKey: { type: String, required: true, unique: true },
  number: {
    type: String, required: true, trim: true, minlength: 8,
    validate: {
      validator: value => /^\d{2,3}-\d+$/.test(value),
      message: 'Number must have 2 or 3 digits, one hyphen, then digits, with at least 8 characters in total',
    },
  },
})

personSchema.pre('validate', function () {
  if (this.name) this.nameKey = this.name.trim().toLowerCase()
})

personSchema.set('toJSON', {
  transform: (_document, result) => {
    result.id = result._id.toString()
    delete result._id
    delete result.__v
    delete result.nameKey
    return result
  },
})

export default mongoose.models.Person || mongoose.model('Person', personSchema)
