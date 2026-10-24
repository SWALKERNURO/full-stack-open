# Part 4: blog list API

Independent Express/Mongoose application covering exercises 4.1-4.23. The final API has separate controllers, models, configuration and middleware; author statistics; database-backed API tests; bcrypt passwords; populated relationships; one-hour JWT authentication; token/user middleware; and creator-only deletion.

## Run

Install dependencies from the repository root. Copy `bloglist/.env.example` to `bloglist/.env` and supply private database and signing-secret values. From `part4/bloglist`, run `npm start` or `npm run dev`. The default port is 3003.

`POST /api/users` registers a username, name and password. `POST /api/login` returns a token. Send `Authorization: Bearer TOKEN` to `POST /api/blogs` or `DELETE /api/blogs/:id`. Public `GET /api/blogs` and `GET /api/users` populate their related records without exposing password hashes. `PUT /api/blogs/:id` changes likes only; creator identity cannot be reassigned through request bodies.

## Verify

Set `TEST_MONGODB_URI` to a disposable database whose name ends in `_test`, then run `npm test --workspace=part4/bloglist`. Tests erase only that dedicated database's blog and user collections. Production and test URIs are deliberately separate. API checks use real MongoDB persistence through Supertest; pure helper tests cover empty lists, totals, favorites and aggregated author statistics. No production credentials are needed in CI.

The test fixtures use fictional users, example.com URLs and disposable passwords. The application implements the course's public registration and public likes behavior; it is an exercise service rather than a hardened public account platform.
