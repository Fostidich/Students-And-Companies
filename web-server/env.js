window.env = window.env || {
  VITE_API_SERVER_URL: process.env.VITE_API_SERVER_URL || import.meta.env.VITE_API_SERVER_URL || 'http://localhost:5000',
};

