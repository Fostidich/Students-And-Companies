import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";

export default defineConfig(({ mode }) => ({
  base: "/",
  plugins: [react()],
  define:
    mode === "development"
      ? {
          "window.env": {
            VITE_API_SERVER_URL:
              process.env.VITE_API_SERVER_URL || "http://localhost:5000",
          },
        }
      : {},
}));
