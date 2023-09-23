import axios from "axios";

const apiCliente = axios.create({
  baseURL: "https://localhost:7247",
  headers: {
    Accept: "*/*",
    "Content-Type": "application/json",
  },
});

const apiAuth = axios.create({
  baseURL: "https://localhost:7106/api/auth",
  headers: {
    Accept: "*/*",
    "Content-Type": "application/json",
  },
});

apiAuth.interceptors.request.use(async (config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export { apiCliente, apiAuth };
export * from "./login";
