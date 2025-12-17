import type {
    CreateTeamPlayerInput,
    ModifyTeamPlayerInput,
    TeamPlayerData,
} from "../types/interfaces";
import api from "./api";

const TeamPlayerService = {
    getAll: () => api.get<TeamPlayerData[]>("TeamPlayers"),
    getById: (id: number) => api.get<TeamPlayerData>(`TeamPlayers/${id}`),
    create: (teamPlayer: CreateTeamPlayerInput) =>
        api.post<TeamPlayerData>("TeamPlayers", teamPlayer),
    update: (id: number, teamPlayer: ModifyTeamPlayerInput) =>
        api.put<TeamPlayerData>(`TeamPlayers/${id}`, teamPlayer),
    delete: (id: number) => api.delete(`TeamPlayers/${id}`),
};

export default TeamPlayerService;
