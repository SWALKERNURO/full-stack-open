import { useState } from 'react'
import { StyledForm } from '../ui.js'

export default function BlogForm({ onCreate, onCancel }) {
  const [title, setTitle] = useState('')
  const [author, setAuthor] = useState('')
  const [url, setUrl] = useState('')
  const [busy, setBusy] = useState(false)
  const submit = async event => {
    event.preventDefault()
    setBusy(true)
    try {
      const created = await onCreate({ title: title.trim(), author: author.trim(), url: url.trim() })
      if (created !== false) { setTitle(''); setAuthor(''); setUrl('') }
    } finally { setBusy(false) }
  }
  return <section aria-labelledby="create-heading">
    <h2 id="create-heading">Create a new blog</h2>
    <StyledForm onSubmit={submit}>
      <label>Title<input value={title} onChange={event => setTitle(event.target.value)} required /></label>
      <label>Author<input value={author} onChange={event => setAuthor(event.target.value)} /></label>
      <label>URL<input type="url" pattern="https?://.+" value={url} onChange={event => setUrl(event.target.value)} required /></label>
      <div className="actions"><button type="submit" disabled={busy}>Create</button><button type="button" onClick={onCancel} disabled={busy}>Cancel</button></div>
    </StyledForm>
  </section>
}
