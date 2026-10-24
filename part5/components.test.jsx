import { afterEach, describe, expect, test, vi } from 'vitest'
import { cleanup, fireEvent, render, screen, waitFor } from '@testing-library/react'
import Blog from './bloglist/src/components/Blog.jsx'
import BlogForm from './bloglist/src/components/BlogForm.jsx'

afterEach(cleanup)
const blog = { id: 'one', title: 'Boundary notes', author: 'Avery', url: 'https://example.com/notes', likes: 3, user: { username: 'avery', name: 'Avery Fixture' } }
const props = { blog, user: { username: 'avery' }, onLike: vi.fn(), onRemove: vi.fn() }
describe('blog display', () => {
  test('shows title and author while hiding URL and likes by default', () => {
    render(<Blog {...props} />)
    expect(screen.getByText(blog.title)).toBeTruthy()
    expect(screen.getByText(blog.author)).toBeTruthy()
    expect(screen.queryByText(blog.url)).toBeNull()
    expect(screen.queryByRole('button', { name: 'Like', exact: true })).toBeNull()
  })
  test('view reveals URL, likes and creator; hide collapses details', () => {
    render(<Blog {...props} />)
    fireEvent.click(screen.getByRole('button', { name: 'View' }))
    expect(screen.getByRole('link').getAttribute('href')).toBe(blog.url)
    expect(screen.getByText('3')).toBeTruthy()
    expect(screen.getByText('Added by Avery Fixture')).toBeTruthy()
    fireEvent.click(screen.getByRole('button', { name: 'Hide' }))
    expect(screen.queryByRole('link')).toBeNull()
  })
  test('two like clicks call the supplied callback twice', () => {
    const onLike = vi.fn()
    render(<Blog {...props} onLike={onLike} />)
    fireEvent.click(screen.getByRole('button', { name: 'View' }))
    fireEvent.click(screen.getByRole('button', { name: 'Like', exact: true }))
    fireEvent.click(screen.getByRole('button', { name: 'Like', exact: true }))
    expect(onLike).toHaveBeenCalledTimes(2)
    expect(onLike).toHaveBeenLastCalledWith(blog)
  })
  test('another user cannot see remove', () => {
    render(<Blog {...props} user={{ username: 'other' }} />)
    fireEvent.click(screen.getByRole('button', { name: 'View' }))
    expect(screen.queryByRole('button', { name: 'Remove' })).toBeNull()
  })
})
describe('blog creation form', () => {
  test('sends the correct fields to the callback and clears on success', async () => {
    const onCreate = vi.fn().mockResolvedValue(true)
    render(<BlogForm onCreate={onCreate} onCancel={vi.fn()} />)
    fireEvent.change(screen.getByLabelText('Title'), { target: { value: 'New entry' } })
    fireEvent.change(screen.getByLabelText('Author'), { target: { value: 'Sam' } })
    fireEvent.change(screen.getByLabelText('URL'), { target: { value: 'https://example.com/new' } })
    fireEvent.click(screen.getByRole('button', { name: 'Create', exact: true }))
    await waitFor(() => expect(onCreate).toHaveBeenCalledWith({ title: 'New entry', author: 'Sam', url: 'https://example.com/new' }))
    await waitFor(() => expect(screen.getByLabelText('Title').value).toBe(''))
  })
  test('a failed save retains the entered fields for retry', async () => {
    const onCreate = vi.fn().mockResolvedValue(false)
    render(<BlogForm onCreate={onCreate} onCancel={vi.fn()} />)
    fireEvent.change(screen.getByLabelText('Title'), { target: { value: 'Keep my draft' } })
    fireEvent.change(screen.getByLabelText('URL'), { target: { value: 'https://example.com/draft' } })
    fireEvent.click(screen.getByRole('button', { name: 'Create', exact: true }))
    await waitFor(() => expect(onCreate).toHaveBeenCalledTimes(1))
    expect(screen.getByLabelText('Title').value).toBe('Keep my draft')
  })
})
