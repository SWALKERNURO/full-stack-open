import axios from 'axios'

const baseUrl = import.meta.env.VITE_API_URL || 'http://localhost:3001/persons'
const getAll = () => axios.get(baseUrl).then(response => response.data)
const create = person => axios.post(baseUrl, person).then(response => response.data)
const update = (id, person) => axios.put(`${baseUrl}/${encodeURIComponent(id)}`, person).then(response => response.data)
const remove = id => axios.delete(`${baseUrl}/${encodeURIComponent(id)}`)

export default { getAll, create, update, remove }
