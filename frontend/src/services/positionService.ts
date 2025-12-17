import type { CreatePositionInput, PositionData } from "../types/interfaces";
import api from "./api";

const PositionService = {
    getAll: () => api.get<PositionData[]>("Positions"),
    getById: (id: number) => api.get<PositionData>(`Positions/${id}`),
    create: (position: CreatePositionInput) =>
        api.post<PositionData>("Positions", position),
    update: (id: number, position: PositionData) =>
        api.put<PositionData>(`Positions/${id}`, position),
    delete: (id: number) => api.delete(`Positions/${id}`),
};

export default PositionService;
