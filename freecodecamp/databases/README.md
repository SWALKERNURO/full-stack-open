# Relational database certification projects

Prepared SQL and Bash source for all five freeCodeCamp v9 database projects. Course submissions remain pending CodeRoad workspace completion.

The CI workflow checks out each official project at the setup commit listed in its tutorial, loads the database and authored scripts, and runs the official tests without editing them. Tests run against an isolated PostgreSQL 12 service. The workflow exports real pg_dump files after verification; test-only customers and guessing-game records are cleared before export.

The periodic-table and number-guessing folders include Base64-encoded Git bundles containing their development history. Decode the bundle with `base64 --decode history.bundle.base64 > project.bundle`, then clone it to the required `periodic_table` or `number_guessing_game` directory in the official workspace. The commits preserve the initial scaffold, database changes, program, tests, and documentation, with the required branch and commit-message conventions.

The celestial catalog is fictional. The World Cup CSV and expected output, periodic-table starting data, and test harnesses come from the official freeCodeCamp projects. Source was implemented with AI assistance. No account token or webhook credential is included.
