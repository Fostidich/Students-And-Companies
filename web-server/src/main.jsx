import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import FirstPage from "./FirstPage.jsx";

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <FirstPage />
  </StrictMode>,
);
