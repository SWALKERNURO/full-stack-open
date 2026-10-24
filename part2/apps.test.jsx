import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { cleanup, fireEvent, render, screen, waitFor, within } from '@testing-library/react'
import Course from './courseinfo/src/Course.jsx'
import CourseApp from './courseinfo/src/App.jsx'
import Phonebook from './phonebook/src/App.jsx'
import personsService from './phonebook/src/services/persons.js'
import Countries from './countries/src/App.jsx'
import { getCountries, getWeather } from './countries/src/services/countries.js'

vi.mock('./phonebook/src/services/persons.js', () => ({ default: {
  getAll: vi.fn(), create: vi.fn(), update: vi.fn(), remove: vi.fn(),
} }))
vi.mock('./countries/src/services/countries.js', () => ({ getCountries: vi.fn(), getWeather: vi.fn() }))

const initialPersons = [
  { id: '1', name: 'Arto Hellas', number: '123-4567' },
  { id: '2', name: 'Ada Lovelace', number: '234-5678' },
]
const makeCountry = (name, id) => ({
  name: { common: name }, cca3: id, capital: [`${name} City`], capitalInfo: { latlng: [60, 25] },
  area: 1234, languages: { en: 'English' }, flags: { svg: '/flag.svg', alt: `${name} flag` },
})
const countryData = [makeCountry('Finland', 'FIN'), makeCountry('Fiji', 'FJI'), makeCountry('Sweden', 'SWE')]

beforeEach(() => {
  vi.clearAllMocks()
  personsService.getAll.mockResolvedValue(initialPersons)
  getCountries.mockResolvedValue(countryData)
  getWeather.mockResolvedValue({ current: { temperature_2m: 8, wind_speed_10m: 12, weather_code: 0 } })
})
afterEach(() => { cleanup(); vi.restoreAllMocks() })

describe('course collections', () => {
  it('renders each course and its own total', () => {
    render(<CourseApp />)
    expect(screen.getByText('total of 42 exercises')).toBeTruthy()
    expect(screen.getByText('total of 10 exercises')).toBeTruthy()
  })
  it('handles arbitrary lengths including an empty parts list', () => {
    const { rerender } = render(<Course course={{ id: 1, name: 'Empty', parts: [] }} />)
    expect(screen.getByText('total of 0 exercises')).toBeTruthy()
    rerender(<Course course={{ id: 1, name: 'Longer', parts: [1, 2, 3, 4, 5].map(n => ({ id: n, name: `Part ${n}`, exercises: n })) }} />)
    expect(screen.getByText('Part 5 5')).toBeTruthy()
    expect(screen.getByText('total of 15 exercises')).toBeTruthy()
  })
})

const fillPerson = (name, number) => {
  fireEvent.change(screen.getByLabelText('name:'), { target: { value: name } })
  fireEvent.change(screen.getByLabelText('number:'), { target: { value: number } })
}

describe('persistent phonebook', () => {
  it('loads records, filters case-insensitively, and saves a new person', async () => {
    personsService.create.mockResolvedValue({ id: '3', name: 'Grace Hopper', number: '345-6789' })
    render(<Phonebook />)
    await screen.findByText(/Arto Hellas 123-4567/)
    fireEvent.change(screen.getByLabelText('filter shown with'), { target: { value: 'ADA' } })
    expect(screen.getByText(/Ada Lovelace 234-5678/)).toBeTruthy()
    expect(screen.queryByText(/Arto Hellas 123-4567/)).toBeNull()
    fireEvent.change(screen.getByLabelText('filter shown with'), { target: { value: '' } })
    fillPerson('Grace Hopper', '345-6789')
    fireEvent.click(screen.getByRole('button', { name: 'add' }))
    await screen.findByRole('status')
    expect(personsService.create).toHaveBeenCalledWith({ name: 'Grace Hopper', number: '345-6789' })
    expect(screen.getByText(/Grace Hopper 345-6789/)).toBeTruthy()
    expect(screen.getByLabelText('name:').value).toBe('')
  })

  it('warns about an unchanged duplicate without adding it', async () => {
    const alert = vi.spyOn(window, 'alert').mockImplementation(() => {})
    render(<Phonebook />)
    await screen.findByText(/Arto Hellas 123-4567/)
    fillPerson('arto hellas', '123-4567')
    fireEvent.click(screen.getByRole('button', { name: 'add' }))
    expect(alert).toHaveBeenCalledOnce()
    expect(personsService.create).not.toHaveBeenCalled()
    expect(personsService.update).not.toHaveBeenCalled()
  })

  it('updates an existing record after confirmation', async () => {
    vi.spyOn(window, 'confirm').mockReturnValue(true)
    personsService.update.mockResolvedValue({ ...initialPersons[0], number: '999-0000' })
    render(<Phonebook />)
    await screen.findByText(/Arto Hellas 123-4567/)
    fillPerson('Arto Hellas', '999-0000')
    fireEvent.click(screen.getByRole('button', { name: 'add' }))
    await screen.findByText(/Arto Hellas 999-0000/)
    expect(personsService.update).toHaveBeenCalledWith('1', { ...initialPersons[0], number: '999-0000' })
    expect(screen.getByRole('status').textContent).toContain('Updated')
  })

  it('removes a stale record and reports a failed update', async () => {
    vi.spyOn(window, 'confirm').mockReturnValue(true)
    personsService.update.mockRejectedValue({ response: { status: 404 } })
    render(<Phonebook />)
    await screen.findByText(/Arto Hellas 123-4567/)
    fillPerson('Arto Hellas', '999-0000')
    fireEvent.click(screen.getByRole('button', { name: 'add' }))
    expect((await screen.findByRole('alert')).textContent).toContain('already removed')
    expect(screen.queryByRole('button', { name: 'delete Arto Hellas' })).toBeNull()
    expect(screen.getByLabelText('number:').value).toBe('999-0000')
  })

  it('honors cancelled deletion, then deletes the confirmed record', async () => {
    const confirm = vi.spyOn(window, 'confirm').mockReturnValue(false)
    personsService.remove.mockResolvedValue({})
    render(<Phonebook />)
    const remove = await screen.findByRole('button', { name: 'delete Ada Lovelace' })
    fireEvent.click(remove)
    expect(personsService.remove).not.toHaveBeenCalled()
    confirm.mockReturnValue(true)
    fireEvent.click(remove)
    await waitFor(() => expect(screen.queryByRole('button', { name: 'delete Ada Lovelace' })).toBeNull())
    expect(personsService.remove).toHaveBeenCalledWith('2')
  })
})

describe('country search and weather', () => {
  it('limits long lists and renders an exact match with weather', async () => {
    getCountries.mockResolvedValue([...countryData, ...Array.from({ length: 9 }, (_, i) => makeCountry(`Other ${i}`, `O${i}`))])
    render(<Countries />)
    await screen.findByText('Too many matches, specify another filter')
    fireEvent.change(screen.getByLabelText('find countries'), { target: { value: 'FINLAND' } })
    expect(screen.getByRole('heading', { name: 'Finland' })).toBeTruthy()
    expect(screen.getByText('area 1234 km�')).toBeTruthy()
    expect(screen.getByText('English')).toBeTruthy()
    expect(screen.getByRole('img', { name: 'Finland flag' })).toBeTruthy()
    expect(await screen.findByText('temperature 8 �C')).toBeTruthy()
    expect(screen.getByText('wind 12 km/h')).toBeTruthy()
  })

  it('opens a chosen match and resets selection when the query changes', async () => {
    render(<Countries />)
    await screen.findByRole('button', { name: 'show Finland' })
    fireEvent.change(screen.getByLabelText('find countries'), { target: { value: 'fi' } })
    expect(screen.getAllByRole('button', { name: /^show/ }).length).toBe(2)
    fireEvent.click(screen.getByRole('button', { name: 'show Fiji' }))
    expect(screen.getByRole('heading', { name: 'Fiji' })).toBeTruthy()
    await screen.findByText('temperature 8 �C')
    fireEvent.change(screen.getByLabelText('find countries'), { target: { value: 'swed' } })
    expect(screen.getByRole('heading', { name: 'Sweden' })).toBeTruthy()
    expect(screen.queryByRole('heading', { name: 'Fiji' })).toBeNull()
    await screen.findByText('temperature 8 �C')
  })

  it('shows no results for an unmatched query', async () => {
    render(<Countries />)
    await screen.findByRole('button', { name: 'show Finland' })
    fireEvent.change(screen.getByLabelText('find countries'), { target: { value: 'unmatched place' } })
    expect(screen.getByText('No countries found')).toBeTruthy()
  })

  it('reports weather failure without hiding country details', async () => {
    getWeather.mockRejectedValue(new Error('offline'))
    render(<Countries />)
    await screen.findByRole('button', { name: 'show Finland' })
    fireEvent.click(screen.getByRole('button', { name: 'show Finland' }))
    expect((await screen.findByRole('alert')).textContent).toContain('Weather')
    expect(within(screen.getByRole('region', { name: 'Country details' })).getByRole('heading', { name: 'Finland' })).toBeTruthy()
  })
})
