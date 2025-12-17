import type {
    CreateDivisionAndFieldData,
    FieldData,
} from "../types/interfaces";
import api from "./api";

const FieldService = {
    getAll: () => api.get<FieldData[]>("Fields"),
    getById: (id: number) => api.get<FieldData>(`Fields/${id}`),
    create: (field: CreateDivisionAndFieldData) =>
        api.post<FieldData>("Fields", field),
    update: (id: number, field: FieldData) =>
        api.put<FieldData>(`Fields/${id}`, field),
    delete: (id: number) => api.delete(`Fields/${id}`),
};

export default FieldService;
