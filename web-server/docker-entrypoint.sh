#!/bin/sh
echo "window.env = { VITE_API_SERVER_URL: '${VITE_API_SERVER_URL}' };" > /usr/share/nginx/html/env.js
exec "$@"
