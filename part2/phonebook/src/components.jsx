export const Notification = ({ message }) => message ? (
  <p role={message.kind === 'error' ? 'alert' : 'status'} className={`notification ${message.kind}`}>
    {message.text}
  </p>
) : null

export const Filter = ({ value, onChange }) => (
  <label>filter shown with <input value={value} onChange={onChange} /></label>
)

export const PersonForm = ({ name, number, onNameChange, onNumberChange, onSubmit, busy, ready }) => (
  <form onSubmit={onSubmit}>
    <div><label>name: <input required value={name} onChange={onNameChange} /></label></div>
    <div><label>number: <input required type="tel" value={number} onChange={onNumberChange} /></label></div>
    <button type="submit" disabled={busy || !ready}>add</button>
  </form>
)

export const Persons = ({ persons, onDelete, busy }) => (
  <ul>
    {persons.map(person => (
      <li key={person.id}>
        {person.name} {person.number}{' '}
        <button disabled={busy} onClick={() => onDelete(person)} aria-label={`delete ${person.name}`}>delete</button>
      </li>
    ))}
  </ul>
)
