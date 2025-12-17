import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type {
    CreateTeamPlayerInput,
    ModifyTeamPlayerInput,
} from "../types/interfaces";
import TeamPlayerService from "../services/teamPlayerService";

export const useTeamPlayers = () => {
    return useQuery({
        queryKey: ["teamplayers"],
        queryFn: async () => {
            const response = await TeamPlayerService.getAll();
            return response.data;
        },
    });
};

export const useCreateTeamPlayer = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (teamplayer: CreateTeamPlayerInput) =>
            TeamPlayerService.create(teamplayer),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["teamplayers"] });
        },
    });
};

export const useModifyTeamPlayer = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: ({
            id,
            teamplayer,
        }: {
            id: number;
            teamplayer: ModifyTeamPlayerInput;
        }) => TeamPlayerService.update(id, teamplayer),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["teamplayers"] });
        },
    });
};

export const useDeleteTeamPlayer = () => {
    const QueryClient = useQueryClient();

    return useMutation({
        mutationFn: (id: number) => TeamPlayerService.delete(id),
        onSuccess: () => {
            QueryClient.invalidateQueries({ queryKey: ["teamplayers"] });
        },
    });
};
