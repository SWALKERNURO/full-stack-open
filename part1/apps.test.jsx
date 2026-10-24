import { afterEach, describe, expect, it, vi } from 'vitest'
import { cleanup, fireEvent, render, screen, within } from '@testing-library/react'
import CourseInfo from './courseinfo/src/App.jsx'
import Unicafe from './unicafe/src/App.jsx'
import Anecdotes, { anecdotes } from './anecdotes/src/App.jsx'

afterEach(() => {
  cleanup()
  vi.restoreAllMocks()
})

describe('course information', () => {
  it('shows all three parts and their total', () => {
    render(<CourseInfo />)
    expect(screen.getByRole('heading').textContent).toBe('Half Stack application development')
    expect(screen.getByText('Fundamentals of React 10')).toBeTruthy()
    expect(screen.getByText('Using props to pass data 7')).toBeTruthy()
    expect(screen.getByText('State of a component 14')).toBeTruthy()
    expect(screen.getByText('Number of exercises 31')).toBeTruthy()
  })
})

describe('feedback', () => {
  it('shows an empty state before feedback and computes mixed statistics', () => {
    render(<Unicafe />)
    expect(screen.getByText('No feedback given')).toBeTruthy()
    expect(screen.queryByRole('table')).toBeNull()
    fireEvent.click(screen.getByRole('button', { name: 'good' }))
    fireEvent.click(screen.getByRole('button', { name: 'good' }))
    fireEvent.click(screen.getByRole('button', { name: 'neutral' }))
    fireEvent.click(screen.getByRole('button', { name: 'bad' }))
    const rows = within(screen.getByRole('table')).getAllByRole('row')
    expect(rows.map(row => within(row).getAllByRole('cell').map(cell => cell.textContent)))
      .toEqual([['good', '2'], ['neutral', '1'], ['bad', '1'], ['all', '4'], ['average', '0.25'], ['positive', '50 %']])
    expect(screen.queryByText('No feedback given')).toBeNull()
  })

  it('handles a purely negative response', () => {
    render(<Unicafe />)
    fireEvent.click(screen.getByRole('button', { name: 'bad' }))
    expect(screen.getByRole('row', { name: 'average -1' })).toBeTruthy()
    expect(screen.getByRole('row', { name: 'positive 0 %' })).toBeTruthy()
  })
})

describe('anecdote voting', () => {
  it('keeps separate vote counts and updates the leader', () => {
    const random = vi.spyOn(Math, 'random').mockReturnValue(0.2)
    render(<Anecdotes />)
    fireEvent.click(screen.getByRole('button', { name: 'vote', exact: true }))
    fireEvent.click(screen.getByRole('button', { name: 'next anecdote' }))
    const selected = within(screen.getByRole('region', { name: 'Selected anecdote' }))
    const leader = within(screen.getByRole('region', { name: 'Most voted anecdote' }))
    expect(selected.getByText(anecdotes[1])).toBeTruthy()
    expect(selected.getByText('has 0 votes')).toBeTruthy()
    fireEvent.click(screen.getByRole('button', { name: 'vote', exact: true }))
    fireEvent.click(screen.getByRole('button', { name: 'vote', exact: true }))
    expect(leader.getByText(anecdotes[1])).toBeTruthy()
    expect(leader.getByText('has 2 votes')).toBeTruthy()
    random.mockReturnValue(0)
    fireEvent.click(screen.getByRole('button', { name: 'next anecdote' }))
    expect(selected.getByText(anecdotes[0])).toBeTruthy()
    expect(selected.getByText('has 1 votes')).toBeTruthy()
  })

  it('can select the final entry and has a valid leader when all votes tie', () => {
    vi.spyOn(Math, 'random').mockReturnValue(0.999)
    render(<Anecdotes />)
    expect(within(screen.getByRole('region', { name: 'Most voted anecdote' })).getByText(anecdotes[0])).toBeTruthy()
    fireEvent.click(screen.getByRole('button', { name: 'next anecdote' }))
    expect(within(screen.getByRole('region', { name: 'Selected anecdote' })).getByText(anecdotes[7])).toBeTruthy()
  })
})
