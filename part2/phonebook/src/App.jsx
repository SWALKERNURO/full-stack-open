import { useEffect, useRef, useState } from 'react'
import personsService from './services/persons.js'
import { Filter, Notification, PersonForm, Persons } from './components.jsx'
import './style.css'

const App = () => {
  const [persons, setPersons] = useState([])
  const [newName, setNewName] = useState('')
  const [newNumber, setNewNumber] = useState('')
  const [filter, setFilter] = useState('')
  const [message, setMessage] = useState(null)
  const [ready, setReady] = useState(false)
  const [busy, setBusy] = useState(false)
  const notificationTimer = useRef(null)

  const notify = (text, kind = 'success') => {
    clearTimeout(notificationTimer.current)
    setMessage({ text, kind })
    notificationTimer.current = setTimeout(() => setMessage(null), 5000)
  }

  useEffect(() => {
    let active = true
    personsService.getAll().then(data => {
      if (active) { setPersons(data); setReady(true) }
    }).catch(() => {
      if (active) setMessage({ text: 'Could not load the phonebook. Check the server and reload.', kind: 'error' })
    })
    return () => { active = false; clearTimeout(notificationTimer.current) }
  }, [])

  const clearForm = () => { setNewName(''); setNewNumber('') }

  const addPerson = async event => {
    event.preventDefault()
    if (busy || !ready) return
    const name = newName.trim()
    const number = newNumber.trim()
    if (!name || !number) return
    const existing = persons.find(person => person.name.toLowerCase() === name.toLowerCase())
    if (existing && existing.number === number) {
      window.alert(`${existing.name} is already added to the phonebook`)
      return
    }
    if (existing && !window.confirm(`Replace the number for ${existing.name}?`)) return

    setBusy(true)
    try {
      if (existing) {
        const saved = await personsService.update(existing.id, { ...existing, number })
        setPersons(current => current.map(person => person.id === saved.id ? saved : person))
        notify(`Updated ${saved.name}`)
      } else {
        const saved = await personsService.create({ name, number })
        setPersons(current => current.concat(saved))
        notify(`Added ${saved.name}`)
      }
      clearForm()
    } catch (error) {
      if (existing && error.response?.status === 404) {
        setPersons(current => current.filter(person => person.id !== existing.id))
        notify(`${existing.name} was already removed from the server`, 'error')
      } else {
        notify(error.response?.data?.error || 'Could not save the person. Please try again.', 'error')
      }
    } finally { setBusy(false) }
  }

  const deletePerson = async person => {
    if (busy || !window.confirm(`Delete ${person.name}?`)) return
    setBusy(true)
    try {
      await personsService.remove(person.id)
      setPersons(current => current.filter(item => item.id !== person.id))
      notify(`Deleted ${person.name}`)
    } catch (error) {
      if (error.response?.status === 404) {
        setPersons(current => current.filter(item => item.id !== person.id))
        notify(`${person.name} was already removed from the server`, 'error')
      } else notify('Could not delete the person. Please try again.', 'error')
    } finally { setBusy(false) }
  }

  const visible = persons.filter(person => person.name.toLowerCase().includes(filter.trim().toLowerCase()))

  return (
    <main>
      <h1>Phonebook</h1>
      <Notification message={message} />
      <Filter value={filter} onChange={event => setFilter(event.target.value)} />
      <h2>Add a new person</h2>
      <PersonForm name={newName} number={newNumber} busy={busy} ready={ready} onSubmit={addPerson}
        onNameChange={event => setNewName(event.target.value)} onNumberChange={event => setNewNumber(event.target.value)} />
      <h2>Numbers</h2>
      {!ready && !message && <p>Loading phonebook.</p>}
      <Persons persons={visible} onDelete={deletePerson} busy={busy} />
    </main>
  )
}

export default App
