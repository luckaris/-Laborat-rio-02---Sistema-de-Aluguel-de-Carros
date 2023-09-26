import axios from "axios";

const apiVehicle = axios.create({
  baseURL: "https://localhost:7060/",
  headers: {
    Accept: "*/*",
    "Content-Type": "application/json",
    "Access-Control-Allow-Origin": "*",
  },
});

const apiClient = axios.create({
  baseURL: "https://localhost:7056",
  headers: {
    Accept: "*/*",
    "Content-Type": "application/json",
    "Access-Control-Allow-Origin": "*",
  },
});

const apiPedidos = axios.create({
  baseURL: "https://localhost:7173",
  headers: {
    Accept: "*/*",
    "Content-Type": "application/json",
    "Access-Control-Allow-Origin": "*",
  },
});

const apiAuth = axios.create({
  baseURL: "https://localhost:7105",
  headers: {
    Accept: "*/*",
    "Content-Type": "application/json",
    "Access-Control-Allow-Origin": "*",
  },
});

apiAuth.interceptors.request.use(async (config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export { apiClient, apiAuth, apiVehicle, apiPedidos };
export * from "./login";
