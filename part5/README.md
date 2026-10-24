# Part 5: blog list frontend and browser tests

React/Vite frontend for the Part 4 API, covering exercises 5.1-5.23. Includes persisted login/logout, timed success/error notifications, a cancellable creation form with independent state, expandable blog details, likes with creator information retained, sorting by likes, and confirmed deletion visible only to the creator.

## Run

Start the Part 4 API on port 3003 with its private environment configured. From the repository root, run `npm run dev --workspace=part5/bloglist`. Vite proxies `/api` to the backend. No database credentials or signing secrets belong in the frontend. This course implementation stores the expiring login token in localStorage as requested by exercise 5.2.

## Verify

`npm test` at the root includes the component tests in `components.test.jsx`. Run `npm run lint --workspace=part5/bloglist` for ESLint.

`npm test --workspace=part5/e2e` runs a separate Playwright project against Chromium. Install the test browser first with `npx playwright install chromium`. The runner starts both applications automatically and uses an isolated `bloglist_e2e_test` database by default. Set `E2E_MONGODB_URI` to select a different disposable database ending in `_test`. It deliberately refuses to reuse an existing application server.

The reset endpoint is registered only in test mode and additionally checks the connected database's `_test` suffix before deleting fixtures. It is absent in normal production mode. The suite covers default login display, valid/invalid login, session persistence and logout, blog creation/persistence, repeated likes, deletion confirmation/cancellation, ownership visibility and ordering.
