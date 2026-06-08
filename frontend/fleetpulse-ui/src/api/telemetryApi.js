import axios from "axios";

const api  = axios.create({
    baseURL: "http://localhost:5069/api"
});

export const getLatestVehicle = (vehicleId) =>
    api.get(`/Telemetry/vehicles/${vehicleId}/latest`);

export const getVehicleHistory = (vehicleId) =>
    api.get(`/Telemetry/vehicles/${vehicleId}/history`);

export const getVehicleSummary = (vehicleId) =>
    api.get(`/Telemetry/vehicles/${vehicleId}/summary`);

export const getVehicleIds = () =>
    api.get("/Telemetry/vehicles");

export const createTelemtryEvent = (event) =>
    api.post("/Telemetry", event);

export default api;