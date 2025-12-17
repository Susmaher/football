import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { CreateRefereeInput, RefereeData } from "../types/interfaces";
import RefereeService from "../services/refereeService";

export const useReferees = () => {
    return useQuery({
        queryKey: ["referees"],
        queryFn: async () => {
            const response = await RefereeService.getAll();
            return response.data;
        },
    });
};

export const useCreateReferee = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (referee: CreateRefereeInput) =>
            RefereeService.create(referee),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["referees"] });
        },
    });
};

export const useModifyReferee = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: ({ id, referee }: { id: number; referee: RefereeData }) =>
            RefereeService.update(id, referee),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["referees"] });
        },
    });
};

export const useDeleteReferee = () => {
    const QueryClient = useQueryClient();

    return useMutation({
        mutationFn: (id: number) => RefereeService.delete(id),
        onSuccess: () => {
            QueryClient.invalidateQueries({ queryKey: ["referees"] });
        },
    });
};
