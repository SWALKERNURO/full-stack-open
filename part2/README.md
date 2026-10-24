# Part 2

- `courseinfo`: exercises 2.1-2.5; arbitrary course and part lists, totals with reduce, separate Course module.
- `phonebook`: exercises 2.6-2.17; controlled form, filtering, Axios CRUD service, confirmations, success/error messages, and stale-entry recovery.
- `countries`: exercises 2.18-2.20; country search, detail selection, flags and languages, plus capital weather from Open-Meteo.

Install dependencies from the repository root with `npm install`. Start each app with `npm run dev --workspace=part2/<name>`.

For the phonebook, also start `npm run server --workspace=part2/phonebook` in another terminal. The JSON server listens on port 3001. Seed records are fictional exercise data. Override `VITE_API_URL` if a different backend is required.

The countries app uses the University of Helsinki countries endpoint and the public Open-Meteo forecast endpoint. No API key is required, and the weather view includes attribution. External-service failures have visible error states.

Run `npm run build` and `npm test` from the repository root.
