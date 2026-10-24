import { useEffect, useRef, useState } from 'react'
import api from './services/api.js'
import Blog from './components/Blog.jsx'
import BlogForm from './components/BlogForm.jsx'
import LoginForm from './components/LoginForm.jsx'

const storageKey = 'fullstackBlogUser'
const restoreUser = () => {
  try {
    const value = JSON.parse(localStorage.getItem(storageKey))
    return value && typeof value.token === 'string' && typeof value.username === 'string' ? value : null
  } catch { return null }
}
export default function App() {
  const [user, setUser] = useState(restoreUser)
  const [blogs, setBlogs] = useState([])
  const [loading, setLoading] = useState(false)
  const [loadError, setLoadError] = useState(false)
  const [retry, setRetry] = useState(0)
  const [showForm, setShowForm] = useState(false)
  const [notice, setNotice] = useState(null)
  const [busyIds, setBusyIds] = useState(new Set())
  const notificationTimer = useRef(null)
  const notify = (text, error = false) => {
    clearTimeout(notificationTimer.current)
    setNotice({ text, error })
    notificationTimer.current = setTimeout(() => setNotice(null), 5000)
  }
  useEffect(() => () => clearTimeout(notificationTimer.current), [])
  useEffect(() => {
    if (!user) return
    let active = true
    const load = async () => {
      setLoading(true); setLoadError(false)
      try { const values = await api.getAll(); if (active) setBlogs(values) }
      catch { if (active) setLoadError(true) }
      finally { if (active) setLoading(false) }
    }
    void load()
    return () => { active = false }
  }, [user, retry])
  const logout = () => {
    try { localStorage.removeItem(storageKey) } catch { /* Session state still clears. */ }
    setUser(null); setBlogs([]); setShowForm(false)
  }
  const handleError = error => {
    if (error.response?.status === 401) { logout(); notify('Your session expired. Please log in again.', true) }
    else notify(error.response?.data?.error || 'The request failed. Please try again.', true)
  }
  const login = async credentials => {
    try {
      const value = await api.login(credentials)
      try { localStorage.setItem(storageKey, JSON.stringify(value)) } catch { /* Login works for this tab. */ }
      setUser(value); notify(`Welcome, ${value.name || value.username}`)
    } catch { notify('Wrong username or password, or the login service is unavailable.', true) }
  }
  const createBlog = async values => {
    try {
      const created = await api.create(values, user.token)
      setBlogs(current => [...current, created]); setShowForm(false)
      notify(`Added ${created.title} by ${created.author}`)
      return true
    } catch (error) { handleError(error); return false }
  }
  const likeBlog = async blog => {
    setBusyIds(current => new Set(current).add(blog.id))
    try {
      const updated = await api.update({ ...blog, likes: blog.likes + 1 }, user.token)
      setBlogs(current => current.map(value => value.id === updated.id ? updated : value))
    } catch (error) { handleError(error) }
    finally { setBusyIds(current => { const next = new Set(current); next.delete(blog.id); return next }) }
  }
  const removeBlog = async blog => {
    if (!window.confirm(`Remove ${blog.title} by ${blog.author}?`)) return
    setBusyIds(current => new Set(current).add(blog.id))
    try {
      await api.remove(blog.id, user.token)
      setBlogs(current => current.filter(value => value.id !== blog.id)); notify(`Removed ${blog.title}`)
    } catch (error) { handleError(error) }
    finally { setBusyIds(current => { const next = new Set(current); next.delete(blog.id); return next }) }
  }
  const sortedBlogs = [...blogs].sort((left, right) => right.likes - left.likes || left.title.localeCompare(right.title))
  return <main>
    {notice && <div className={notice.error ? 'notice error' : 'notice'} role={notice.error ? 'alert' : 'status'}>{notice.text}</div>}
    {!user ? <LoginForm onLogin={login} /> : <>
      <header><h1>Blog list</h1><p>{user.name || user.username} logged in <button type="button" onClick={logout}>Log out</button></p></header>
      {showForm ? <BlogForm onCreate={createBlog} onCancel={() => setShowForm(false)} /> : <button type="button" onClick={() => setShowForm(true)}>Create new blog</button>}
      {loading && <p role="status">Loading blogs.</p>}
      {loadError && <p role="alert">Could not load blogs. <button type="button" onClick={() => setRetry(value => value + 1)}>Retry</button></p>}
      {!loading && !loadError && blogs.length === 0 && <p>No blogs yet. Add the first one.</p>}
      <section aria-label="Blogs" className="blogs">{sortedBlogs.map(blog => <Blog key={blog.id} blog={blog} user={user} onLike={likeBlog} onRemove={removeBlog} busy={busyIds.has(blog.id)} />)}</section>
    </>}
  </main>
}
