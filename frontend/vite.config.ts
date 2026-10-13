import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// O proxy repassa as chamadas para a API .NET. Assim não precisamos configurar CORS.
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/sessoes': 'http://localhost:5019',
      '/configuracao': 'http://localhost:5019',
    },
  },
})
