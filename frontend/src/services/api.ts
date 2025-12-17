import axios from "axios";

const API_BASE_URL = "https://localhost:7129/api/";

const api = axios.create({
    baseURL: API_BASE_URL,
    withCredentials: true,
    headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
    },
});

export const getDivisions = async () => {
    const response = await api.get("Divisions");
    return response.data;
};

export default api;
