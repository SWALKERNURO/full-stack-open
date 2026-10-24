import { useState } from 'react'
import { StyledForm } from '../ui.js'

export default function LoginForm({ onLogin }) {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [busy, setBusy] = useState(false)
  const submit = async event => {
    event.preventDefault()
    setBusy(true)
    try { await onLogin({ username, password }) } finally { setBusy(false); setPassword('') }
  }
  return <section aria-labelledby="login-heading">
    <h1 id="login-heading">Log in to blog list</h1>
    <StyledForm onSubmit={submit}>
      <label>Username<input autoComplete="username" value={username} onChange={event => setUsername(event.target.value)} required /></label>
      <label>Password<input type="password" autoComplete="current-password" value={password} onChange={event => setPassword(event.target.value)} required /></label>
      <button disabled={busy} type="submit">{busy ? 'Logging in.' : 'Log in'}</button>
    </StyledForm>
  </section>
}
