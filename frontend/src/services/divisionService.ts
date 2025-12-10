import type {
    CreateDivisionAndFieldData,
    DivisionData,
} from "../types/interfaces";
import api from "./api";

const DivisionService = {
    getAll: () => api.get("Divisions"),
    getById: (id: number) => api.get<DivisionData>(`Divisions/${id}`),
    create: (division: CreateDivisionAndFieldData) =>
        api.post<DivisionData>("Divisions", division),
    update: (id: number, division: DivisionData) =>
        api.put<DivisionData>(`Divisions/${id}`, division),
    delete: (id: number) => api.delete(`Divisions/${id}`),
};

export default DivisionService;
