import { useEffect, useRef, useState } from 'react'
import { Link, NavLink, Navigate, Route, Routes, useNavigate, useParams } from 'react-router-dom'
import api from './services/api.js'
import BlogDetails from './components/BlogDetails.jsx'
import BlogForm from './components/BlogForm.jsx'
import LoginForm from './components/LoginForm.jsx'
import { Navigation, Notification } from './ui.js'

const storageKey = 'fullstackBlogUser'
const restoreUser = () => {
  try {
    const value = JSON.parse(localStorage.getItem(storageKey))
    return value && typeof value.token === 'string' && typeof value.username === 'string' ? value : null
  } catch { return null }
}
function SingleBlog({ blogs, loading, loadError, ...props }) {
  const { id } = useParams()
  const blog = blogs.find(value => value.id === id)
  if (loading) return <p role="status">Loading blog.</p>
  if (loadError) return <p role="alert">Could not load this blog. Please retry from the blog list.</p>
  if (!blog) return <p>Blog not found. <Link to="/">All blogs</Link></p>
  return <BlogDetails blog={blog} busy={props.busyIds.has(blog.id)} user={props.user} onLike={props.onLike} onRemove={props.onRemove} />
}
export default function App() {
  const navigate = useNavigate()
  const [user, setUser] = useState(restoreUser)
  const [blogs, setBlogs] = useState([])
  const [loading, setLoading] = useState(true)
  const [loadError, setLoadError] = useState(false)
  const [retry, setRetry] = useState(0)
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
    let active = true
    const load = async () => {
      setLoading(true); setLoadError(false)
      try { const values = await api.getAll(); if (active) setBlogs(values) }
      catch { if (active) setLoadError(true) }
      finally { if (active) setLoading(false) }
    }
    void load()
    return () => { active = false }
  }, [retry])
  const logout = () => {
    try { localStorage.removeItem(storageKey) } catch { /* Session state still clears. */ }
    setUser(null); navigate('/')
  }
  const handleError = error => {
    if (error.response?.status === 401) { logout(); notify('Your session expired. Please log in again.', true) }
    else notify(error.response?.data?.error || 'The request failed. Please try again.', true)
  }
  const login = async credentials => {
    try {
      const value = await api.login(credentials)
      try { localStorage.setItem(storageKey, JSON.stringify(value)) } catch { /* Login works for this tab. */ }
      setUser(value); notify(`Welcome, ${value.name || value.username}`); navigate('/')
    } catch { notify('Wrong username or password, or the login service is unavailable.', true) }
  }
  const createBlog = async values => {
    if (!user) return false
    try {
      const created = await api.create(values, user.token)
      setBlogs(current => [...current, created])
      notify(`Added ${created.title} by ${created.author}`); navigate('/')
      return true
    } catch (error) { handleError(error); return false }
  }
  const likeBlog = async blog => {
    if (!user || busyIds.has(blog.id)) return
    setBusyIds(current => new Set(current).add(blog.id))
    try {
      const updated = await api.update({ ...blog, likes: blog.likes + 1 }, user.token)
      setBlogs(current => current.map(value => value.id === updated.id ? updated : value))
    } catch (error) { handleError(error) }
    finally { setBusyIds(current => { const next = new Set(current); next.delete(blog.id); return next }) }
  }
  const removeBlog = async blog => {
    if (!user || user.username !== blog.user?.username || !window.confirm(`Remove ${blog.title} by ${blog.author}?`)) return
    setBusyIds(current => new Set(current).add(blog.id))
    try {
      await api.remove(blog.id, user.token)
      setBlogs(current => current.filter(value => value.id !== blog.id)); notify(`Removed ${blog.title}`); navigate('/')
    } catch (error) { handleError(error) }
    finally { setBusyIds(current => { const next = new Set(current); next.delete(blog.id); return next }) }
  }
  const sortedBlogs = [...blogs].sort((left, right) => right.likes - left.likes || left.title.localeCompare(right.title))
  return <main>
    <Navigation aria-label="Main navigation">
      <NavLink to="/" end>All blogs</NavLink>
      {user ? <><NavLink to="/create">Create new blog</NavLink><span>{user.name || user.username} logged in</span><button type="button" onClick={logout}>Log out</button></> : <NavLink to="/login">Log in</NavLink>}
    </Navigation>
    {notice && <Notification $error={notice.error} role={notice.error ? 'alert' : 'status'}>{notice.text}</Notification>}
    <Routes>
      <Route path="/" element={<><h1>Blog list</h1>
        {loading && <p role="status">Loading blogs.</p>}
        {loadError && <p role="alert">Could not load blogs. <button type="button" onClick={() => setRetry(value => value + 1)}>Retry</button></p>}
        {!loading && !loadError && blogs.length === 0 && <p>No blogs yet.</p>}
        <section aria-label="Blogs" className="blogs">{sortedBlogs.map(blog => <article className="blog" aria-label={blog.title} key={blog.id}><Link to={`/blogs/${blog.id}`}>{blog.title}</Link> <span>{blog.author}</span></article>)}</section>
      </>} />
      <Route path="/login" element={user ? <Navigate to="/" replace /> : <LoginForm onLogin={login} />} />
      <Route path="/create" element={user ? <BlogForm onCreate={createBlog} onCancel={() => navigate('/')} /> : <Navigate to="/login" replace />} />
      <Route path="/blogs/:id" element={<SingleBlog blogs={blogs} loading={loading} loadError={loadError} user={user} onLike={likeBlog} onRemove={removeBlog} busyIds={busyIds} />} />
      <Route path="*" element={<p>Page not found. <Link to="/">All blogs</Link></p>} />
    </Routes>
  </main>
}
