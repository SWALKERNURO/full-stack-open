# Back-end certification project source

Implementation for the five freeCodeCamp v9 back-end certification projects. **Prepared source; course submissions are still pending.**

The GitHub Actions workflow overlays these files onto the official course repository at commit `0b4205fd428f5e9b1e9a0460e3b0bd0486f057d8`, installs the course dependencies, and executes the published project assertions unchanged against the running servers. It also checks prime inputs, timestamps, chat asset delivery and malformed messages, and watchlist validation and permissions. This verifies the code without changing course records.

## Run in the official workspace

Copy each project directory into its matching directory of the official [course repository](https://github.com/freeCodeCamp/back-end-development-and-apis), retaining its other files. Use Node.js 24 and `npm install` within each project. Start projects one at a time with `npm start`; the profile and watchlist APIs both use port 3000.

The watchlist API requires `JWT_SECRET` in the environment. Copy `.env.example` to `.env` and replace its example secret for local use. The supplied users and watchlists are fictional course fixtures. The server verifies bcrypt hashes and does not use the fixture's plain-text password metadata for authentication.

The chat browser client uses its current host and chooses `wss:` on HTTPS so it works through a Codespaces forwarded port. The server serves the provided HTML and JavaScript separately. It rejects malformed messages and takes the sender's display name from the connection.

Source was implemented with AI assistance. Starter pages, package metadata, database utilities and seeded data originate from the official freeCodeCamp course. No real credentials or account export are included.
