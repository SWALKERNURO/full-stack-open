# freeCodeCamp certification projects

AI-assisted source for the 30 required projects across the six requested freeCodeCamp v9 certifications. Example names, transactions, and form contents are fictional. No personal account export, progress token, or real authentication credential is included. Official starter files, fixture data, images, and video are credited where used.

## Progress

| Curriculum | Projects submitted on freeCodeCamp | Published project checks passed |
| --- | ---: | ---: |
| Responsive Web Design | 5 of 5 | 162 |
| JavaScript | 5 of 5 | 126 |
| Python | 5 of 5 | 103 |
| Front-End Development Libraries | 5 of 5 | 96 |
| Back-End Development and APIs | 5 of 5 | 51 in the official course workspace and CI |
| Relational Databases | 0 of 5; source and SQL dumps ready | 102 in CI |

**25 projects are submitted. The five database projects have verified source and SQL dumps; their official submissions are blocked by an empty CodeRoad view after the database workspace restart. The celestial database passed all 24 checks before restart, but its pass did not sync because the progress secret was not yet loaded. No certification exam has been recorded as passed.**

The back-end verifier runs the published assertions unchanged against the prepared applications overlaid onto the official course repository. Database CI runs the official test suites against isolated PostgreSQL 12 databases, including the required source-control checks. CI does not alter freeCodeCamp completion records or invoke progress webhooks.

Verification runs: [back-end](https://github.com/SWALKERNURO/full-stack-open/actions/runs/40148414478), [databases](https://github.com/SWALKERNURO/full-stack-open/actions/runs/40149326954).

See each folder's README for setup. The freeCodeCamp editor is the reference runtime for browser projects. JSX and TypeScript require the compilation support supplied by that editor or an equivalent local toolchain.
