import { test, expect } from '@playwright/test'

const owner = { username: 'avery', password: 'test-password', name: 'Avery Fixture' }
const other = { username: 'blair', password: 'other-password', name: 'Blair Fixture' }
const login = async (page, user = owner) => {
  await page.getByLabel('Username').fill(user.username)
  await page.getByLabel('Password', { exact: true }).fill(user.password)
  await page.getByRole('button', { name: 'Log in', exact: true }).click()
  await expect(page.getByText(`${user.name} logged in`, { exact: false })).toBeVisible()
}
const createBlog = async (page, title = 'Browser testing notes') => {
  await page.getByRole('button', { name: 'Create new blog' }).click()
  await page.getByLabel('Title', { exact: true }).fill(title)
  await page.getByLabel('Author', { exact: true }).fill('Avery')
  await page.getByLabel('URL', { exact: true }).fill('https://example.com/browser-tests')
  await page.getByRole('button', { name: 'Create', exact: true }).click()
  const blog = page.getByRole('article', { name: title, exact: true })
  await expect(blog).toBeVisible()
  return blog
}
test.beforeEach(async ({ page, request }) => {
  const reset = await request.post('/api/testing/reset')
  expect(reset.status()).toBe(204)
  expect((await request.post('/api/users', { data: owner })).status()).toBe(201)
  expect((await request.post('/api/users', { data: other })).status()).toBe(201)
  await page.goto('/')
})
test('login form is initially shown and blogs are hidden', async ({ page }) => {
  await expect(page.getByRole('heading', { name: 'Log in to blog list' })).toBeVisible()
  await expect(page.getByLabel('Username')).toBeVisible()
  await expect(page.getByLabel('Password', { exact: true })).toBeVisible()
  await expect(page.getByRole('region', { name: 'Blogs', exact: true })).toHaveCount(0)
})
test('correct login persists across reload; logout removes the stored session', async ({ page }) => {
  await login(page)
  await page.reload()
  await expect(page.getByText('Avery Fixture logged in', { exact: false })).toBeVisible()
  await page.getByRole('button', { name: 'Log out' }).click()
  await page.reload()
  await expect(page.getByRole('heading', { name: 'Log in to blog list' })).toBeVisible()
})
test('incorrect password shows an error and does not log in', async ({ page }) => {
  await page.getByLabel('Username').fill(owner.username)
  await page.getByLabel('Password', { exact: true }).fill('wrong-password')
  await page.getByRole('button', { name: 'Log in', exact: true }).click()
  await expect(page.getByRole('alert')).toContainText('Wrong username or password')
  await expect(page.getByRole('heading', { name: 'Log in to blog list' })).toBeVisible()
})
test('logged-in user creates a blog, sees notification and hidden form afterward', async ({ page }) => {
  await login(page)
  await createBlog(page)
  await expect(page.getByRole('status')).toContainText('Added Browser testing notes')
  await expect(page.getByRole('button', { name: 'Create new blog' })).toBeVisible()
  await page.reload()
  await expect(page.getByRole('article', { name: 'Browser testing notes', exact: true })).toBeVisible()
})
test('a blog can be liked repeatedly without losing creator details', async ({ page }) => {
  await login(page)
  const blog = await createBlog(page)
  await blog.getByRole('button', { name: 'View' }).click()
  await blog.getByRole('button', { name: 'Like', exact: true }).click()
  await expect(blog.locator('.like-count')).toHaveText('1')
  await blog.getByRole('button', { name: 'Like', exact: true }).click()
  await expect(blog.locator('.like-count')).toHaveText('2')
  await expect(blog.getByText('Added by Avery Fixture')).toBeVisible()
})
test('owner can cancel deletion then confirm deletion', async ({ page }) => {
  await login(page)
  const blog = await createBlog(page)
  await blog.getByRole('button', { name: 'View' }).click()
  page.once('dialog', dialog => dialog.dismiss())
  await blog.getByRole('button', { name: 'Remove' }).click()
  await expect(blog).toBeVisible()
  page.once('dialog', dialog => dialog.accept())
  await blog.getByRole('button', { name: 'Remove' }).click()
  await expect(blog).toHaveCount(0)
  await page.reload()
  await expect(page.getByRole('article')).toHaveCount(0)
})
test('only the creator sees the remove button', async ({ page }) => {
  await login(page)
  await createBlog(page)
  await page.getByRole('button', { name: 'Log out' }).click()
  await login(page, other)
  const blog = page.getByRole('article', { name: 'Browser testing notes', exact: true })
  await blog.getByRole('button', { name: 'View' }).click()
  await expect(blog.getByRole('button', { name: 'Remove' })).toHaveCount(0)
  await expect(blog.getByRole('button', { name: 'Like', exact: true })).toBeVisible()
})
test('blogs stay sorted by likes, highest first', async ({ page, request }) => {
  const signedIn = await request.post('/api/login', { data: { username: owner.username, password: owner.password } })
  const { token } = await signedIn.json()
  for (const [title, likes] of [['Low', 1], ['High', 9], ['Middle', 4]]) {
    const created = await request.post('/api/blogs', { headers: { Authorization: `Bearer ${token}` }, data: { title, author: 'Fixture', url: 'https://example.com/sorting', likes } })
    expect(created.status()).toBe(201)
  }
  await login(page)
  const articles = page.getByRole('article')
  await expect(articles).toHaveCount(3)
  await expect(articles.nth(0)).toHaveAttribute('aria-label', 'High')
  await expect(articles.nth(1)).toHaveAttribute('aria-label', 'Middle')
  await expect(articles.nth(2)).toHaveAttribute('aria-label', 'Low')
})
