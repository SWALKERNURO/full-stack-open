import { defineConfig } from 'vitest/config'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  test: { environment: 'jsdom', include: ['part[125]/**/*.test.jsx', 'part3/**/*.test.js'] },
})
