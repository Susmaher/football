import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { CreatePositionInput, PositionData } from "../types/interfaces";
import PositionService from "../services/positionService";

export const usePositions = () => {
    return useQuery({
        queryKey: ["positions"],
        queryFn: async () => {
            const response = await PositionService.getAll();
            return response.data;
        },
    });
};

export const useCreatePosition = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (position: CreatePositionInput) =>
            PositionService.create(position),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["positions"] });
        },
    });
};

export const useModifyPosition = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: ({
            id,
            position,
        }: {
            id: number;
            position: PositionData;
        }) => PositionService.update(id, position),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["positions"] });
        },
    });
};

export const useDeletePosition = () => {
    const QueryClient = useQueryClient();

    return useMutation({
        mutationFn: (id: number) => PositionService.delete(id),
        onSuccess: () => {
            QueryClient.invalidateQueries({ queryKey: ["positions"] });
        },
    });
};
