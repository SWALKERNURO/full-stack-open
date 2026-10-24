# Part 1

Final versions of the three exercise applications:

| Exercises | Application | Behavior |
| --- | --- | --- |
| 1.1-1.5 | courseinfo | Course object passed to Header, Content, Part and Total components |
| 1.6-1.11 | unicafe | Feedback counts, average score and positive percentage; empty state and table |
| 1.12-1.14 | anecdotes | Random selection, immutable per-anecdote votes and highest-voted entry |

From the repository root, run `npm install`, then start one application:

```
npm run dev --workspace=part1/courseinfo
npm run dev --workspace=part1/unicafe
npm run dev --workspace=part1/anecdotes
```

Run all production builds with `npm run build` and interaction checks with `npm test`.

The anecdote examples are original short programming observations.
