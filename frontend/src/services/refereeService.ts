import type { CreateRefereeInput, RefereeData } from "../types/interfaces";
import api from "./api";

const RefereeService = {
    getAll: () => api.get<RefereeData[]>("Referees"),
    getById: (id: number) => api.get<RefereeData>(`Referees/${id}`),
    create: (referee: CreateRefereeInput) =>
        api.post<RefereeData>("Referees", referee),
    update: (id: number, referee: RefereeData) =>
        api.put<RefereeData>(`Referees/${id}`, referee),
    delete: (id: number) => api.delete(`Referees/${id}`),
};

export default RefereeService;
