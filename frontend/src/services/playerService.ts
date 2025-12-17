import type {
    CreatePlayerInput,
    ModifyPlayerInput,
    PlayerData,
} from "../types/interfaces";
import api from "./api";

const PlayerService = {
    getAll: (positionId?: number) => {
        const params = positionId ? { position: positionId } : {};
        return api.get<PlayerData[]>("Players", { params });
    },
    getById: (id: number) => api.get<PlayerData>(`Players/${id}`),
    create: (player: CreatePlayerInput) =>
        api.post<PlayerData>("Players", player),
    update: (id: number, player: ModifyPlayerInput) =>
        api.put<PlayerData>(`Players/${id}`, player),
    delete: (id: number) => api.delete(`Players/${id}`),
};

export default PlayerService;
