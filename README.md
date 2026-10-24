# Full Stack Open

Course exercise implementations and submissions.

Core course (Parts 0–5): all 116 exercises submitted, grade 5, certificate available. Parts 6 onward now use the new MOOC platform.

| Part | Work | Submission status |
| --- | --- | --- |
| 0 | Web fundamentals and sequence diagrams, 6 exercises | Submitted |
| 1 | Course information, feedback and anecdote applications, 14 exercises | Submitted |
| 2 | Multiple courses, persistent phonebook and country/weather search, 20 exercises | Submitted |
| 3 | Express/MongoDB phonebook with integrated frontend, 22 exercises | Submitted; live deployment verified |
| 4 | Blog API, author helpers, users, authentication and API tests, 23 exercises | Submitted |
| 5 | Routed and styled blog frontend, component/browser tests, 31 exercises | Submitted |
| 6 | [Zustand and TanStack Query applications](https://github.com/SWALKERNURO/fs-statemanagement), 22 exercises | All six required workflows passed; submission pending |
| 7 | [Custom hooks and extended blog application](https://github.com/SWALKERNURO/fs-extension), 20 exercises | 78 tests passed; MOOC platform submission pending |

The [Check coursework workflow](https://github.com/SWALKERNURO/full-stack-open/actions/workflows/check-coursework.yml) builds the applications, runs lint checks, verifies frontend interactions and backend persistence against disposable MongoDB databases, and runs the Part 5 application in Chromium. Check the latest run for the current verification result.

[Live Part 3 phonebook](https://fullstack-course-phonebook.vercel.app/)

Each part has its own run instructions. Install all dependencies with `npm install` at this root. Do not commit `.env` files, real database passwords or signing secrets. The automated tests use fictional records and dedicated test databases.

[Human workload estimates](WORKLOAD_ESTIMATES.md) are planning estimates, separately labeled from tracked study time.
