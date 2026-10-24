import { defineConfig, devices } from '@playwright/test'
import path from 'node:path'

const root = path.resolve(import.meta.dirname, '../..')
export default defineConfig({
  testDir: './tests', fullyParallel: false, workers: 1, retries: 0,
  timeout: 30000, globalTimeout: 300000,
  reporter: [['list'], ['html', { open: 'never' }]],
  use: { baseURL: 'http://127.0.0.1:5173', trace: 'retain-on-failure', screenshot: 'only-on-failure' },
  projects: [{ name: 'chromium', use: { ...devices['Desktop Chrome'] } }],
  webServer: [
    {
      command: 'npm start --workspace=part4/bloglist', cwd: root,
      url: 'http://127.0.0.1:3003/health', reuseExistingServer: false,
      env: {
        NODE_ENV: 'test', PORT: '3003',
        TEST_MONGODB_URI: process.env.E2E_MONGODB_URI || 'mongodb://127.0.0.1:27017/bloglist_e2e_test',
        SECRET: 'e2e-test-fixture-only-no-production-access-123456',
      },
    },
    {
      command: 'npm run dev --workspace=part5/bloglist -- --host 127.0.0.1 --port 5173 --strictPort',
      cwd: root, url: 'http://127.0.0.1:5173', reuseExistingServer: false,
    },
  ],
})
