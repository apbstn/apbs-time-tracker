import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5220", // Ensure this is correct
  headers: {
    "Content-Type": "application/json",
  },
});

// Load token from storage if available
const token = localStorage.getItem("accessToken");
if (token) {
  api.defaults.headers.common["Authorization"] = `Bearer ${token}`;
}

export default api;
