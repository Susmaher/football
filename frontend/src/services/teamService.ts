import type {
    CreateTeamInputs,
    ModifyTeamInputs,
    TeamData,
} from "../types/interfaces";
import api from "./api";

const TeamService = {
    getAll: (divisionId?: number) => {
        const params = divisionId ? { division: divisionId } : {};
        return api.get<TeamData[]>("Teams", { params });
    },
    getById: (id: number) => api.get<TeamData>(`Teams/${id}`),
    create: (team: CreateTeamInputs) => api.post<TeamData>("Teams", team),
    update: (id: number, team: ModifyTeamInputs) =>
        api.put<TeamData>(`Teams/${id}`, team),
    delete: (id: number) => api.delete(`teams/${id}`),
};

export default TeamService;
