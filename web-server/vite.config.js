import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react-swc';

export default defineConfig(({ mode }) => {
  if (mode === 'development')
    return {
      plugins: [react()],
      define: {
        'window.env': {
          VITE_API_SERVER_URL: process.env.VITE_API_SERVER_URL || 'http://localhost:5000',
        },
      },
    };
  else
    return {
      plugins: [react()],
    };
});
