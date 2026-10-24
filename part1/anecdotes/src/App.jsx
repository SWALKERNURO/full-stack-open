import { useState } from 'react'

export const anecdotes = [
  'A small failing test is a better map than a large guess.',
  'The comment that matters most explains why the obvious solution failed.',
  'Every shortcut eventually asks for its own maintenance budget.',
  'Readable code lets tomorrow\'s programmer start where today\'s programmer stopped.',
  'A reproducible bug is already a more useful bug.',
  'Naming a thing well is a gift to the next person who changes it.',
  'When a function needs a long introduction, it may need a shorter job.',
  'The fastest fix is the one you can still explain next week.',
]

const Anecdote = ({ text, votes }) => (
  <div><p>{text}</p><p>has {votes} votes</p></div>
)

const App = () => {
  const [selected, setSelected] = useState(0)
  const [votes, setVotes] = useState(Array(anecdotes.length).fill(0))
  const mostVoted = votes.indexOf(Math.max(...votes))

  const vote = () => {
    setVotes(previous => {
      const next = [...previous]
      next[selected] += 1
      return next
    })
  }

  return (
    <main>
      <h1>Anecdote of the day</h1>
      <section aria-label="Selected anecdote">
        <Anecdote text={anecdotes[selected]} votes={votes[selected]} />
        <button onClick={vote}>vote</button>
        <button onClick={() => setSelected(Math.floor(Math.random() * anecdotes.length))}>
          next anecdote
        </button>
      </section>
      <h2>Anecdote with most votes</h2>
      <section aria-label="Most voted anecdote">
        <Anecdote text={anecdotes[mostVoted]} votes={votes[mostVoted]} />
      </section>
    </main>
  )
}

export default App
