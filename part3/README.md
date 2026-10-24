# Part 3 phonebook backend

Express 5 backend with MongoDB/Mongoose persistence, validation, Morgan logging, centralized errors, and an integrated production frontend.

## Run locally

1. Install dependencies at the repository root with `npm install`.
2. Copy `phonebook/.env.example` to `phonebook/.env` and configure `MONGODB_URI` for a local MongoDB instance or a dedicated Atlas database.
3. Run `npm run build --workspace=part3/phonebook` to build the Part 2 frontend into the backend's `dist` folder.
4. Run `npm start --workspace=part3/phonebook`. Visit `http://localhost:3001/`.

For frontend development, set `VITE_API_URL=http://localhost:3001/api/persons` before running the Part 2 phonebook Vite server. The Part 2 JSON-server default remains available when using that exercise on its own.

Use `npm run dev --workspace=part3/phonebook` for backend watch mode and `npm run lint --workspace=part3/phonebook` for lint checks.

## Routes

`GET /api/persons`, `GET /api/persons/:id`, `POST /api/persons`, `PUT /api/persons/:id`, `DELETE /api/persons/:id`, `GET /info`, and `GET /health`.

Name validation requires at least three characters. Numbers require at least eight characters in total and the form `NN-NNN...` or `NNN-NNN...`. Updates also run validation. Duplicate names are rejected case-insensitively. Invalid IDs, missing entries and validation failures receive explicit status codes.

## Command-line database exercise

In `part3/phonebook`, configure the private `MONGODB_URI_TEMPLATE` with a `<password>` placeholder, then use `node mongo.js PASSWORD "Sample Person" 040-1234567` to add a record or `node mongo.js PASSWORD` to list records. Never commit a connection password.

## Deployment status

Live application: https://fullstack-course-phonebook.vercel.app/

The application uses MongoDB Atlas persistence. Vercel serves the generated frontend and routes `/api/*`, `/info` and `/health` to the Express application under the same origin. Local execution continues to serve `dist` through Express.

Verification includes live creation, persistence across browser reloads, and rejection of an invalid phone number. CI verifies the full API create/read/update/delete lifecycle, error handling, validation and the command-line add/list program against real MongoDB. The successful check run is https://github.com/SWALKERNURO/full-stack-open/actions/runs/40140374117 .

Deploy from root directory `part3/phonebook`; the checked-in Vercel configuration installs the monorepo's build dependencies and publishes the generated `dist` folder. Set `MONGODB_URI` as a private, sensitive environment variable. Never put it in the frontend or version control.

The approved temporary Atlas network rule expires on 2026-10-31. The deployed database connection needs an appropriate access-list entry after that expiry.
