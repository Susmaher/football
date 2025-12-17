import {
    type CreatePlayerInput,
    type ModifyPlayerInput,
} from "./../types/interfaces";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import PlayerService from "../services/playerService";

export const usePlayers = (positionId?: number) => {
    return useQuery({
        queryKey: ["players"],
        queryFn: async () => {
            const response = await PlayerService.getAll(positionId);
            return response.data;
        },
    });
};

export const useCreatePlayer = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (player: CreatePlayerInput) => PlayerService.create(player),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["players"] });
        },
    });
};

export const useModifyPlayer = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: ({
            id,
            player,
        }: {
            id: number;
            player: ModifyPlayerInput;
        }) => PlayerService.update(id, player),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["players"] });
        },
    });
};

export const useDeletePlayer = () => {
    const QueryClient = useQueryClient();

    return useMutation({
        mutationFn: (id: number) => PlayerService.delete(id),
        onSuccess: () => {
            QueryClient.invalidateQueries({ queryKey: ["players"] });
        },
    });
};
