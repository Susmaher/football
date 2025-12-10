//import { StrictMode } from 'react'
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.tsx";
import { BrowserRouter } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

const queryCilent = new QueryClient();

createRoot(document.getElementById("root")!).render(
    <QueryClientProvider client={queryCilent}>
        <BrowserRouter>
            <App />
        </BrowserRouter>
    </QueryClientProvider>
);
