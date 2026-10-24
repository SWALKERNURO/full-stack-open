import { spawnSync } from 'node:child_process'
import path from 'node:path'

if (!process.env.npm_execpath) throw new Error('Run the build with npm run build')
const destination = path.join(import.meta.dirname, 'dist')
const result = spawnSync(process.execPath, [process.env.npm_execpath, 'run', 'build', '--workspace=part2/phonebook', '--', '--outDir', destination, '--emptyOutDir'], {
  cwd: path.resolve(import.meta.dirname, '../..'),
  env: { ...process.env, VITE_API_URL: '/api/persons' },
  stdio: 'inherit',
})
if (result.error) throw result.error
process.exitCode = result.status ?? 1
