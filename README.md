# Full Stack Open

Course exercise implementations and submissions.

| Part | Work | Submission status |
| --- | --- | --- |
| 0 | Web fundamentals and sequence diagrams, 6 exercises | Submitted |
| 1 | Course information, feedback and anecdote applications, 14 exercises | Submitted |
| 2 | Multiple courses, persistent phonebook and country/weather search, 20 exercises | Submitted |
| 3 | Express/MongoDB phonebook with integrated frontend, 22 exercises | Awaiting Atlas provisioning, internet deployment and live verification |
| 4 | Blog API, author helpers, users, authentication and API tests, 23 exercises | Implemented; awaiting earlier part's submission |
| 5 | Blog frontend, component tests and Chromium end-to-end tests, 23 exercises | Implemented; awaiting earlier parts' submissions |

The [Check coursework workflow](https://github.com/SWALKERNURO/full-stack-open/actions/workflows/check-coursework.yml) builds the applications, runs lint checks, verifies frontend interactions and backend persistence against disposable MongoDB databases, and runs the Part 5 application in Chromium. Check the latest run for the current verification result.

Each part has its own run instructions. Install all dependencies with `npm install` at this root. Do not commit `.env` files, real database passwords or signing secrets. The automated tests use fictional records and dedicated test databases.

[Human workload estimates](WORKLOAD_ESTIMATES.md) are planning estimates, separately labeled from tracked study time.
