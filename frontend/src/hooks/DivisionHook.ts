import type {
    CreateDivisionAndFieldData,
    DivisionData,
} from "./../types/interfaces";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import DivisionService from "../services/divisionService";

export const useDivisions = () => {
    return useQuery({
        queryKey: ["divisions"],
        queryFn: async () => {
            const response = await DivisionService.getAll();
            return response.data;
        },
    });
};

export const useCreateDivision = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (division: CreateDivisionAndFieldData) =>
            DivisionService.create(division),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["divisions"] });
        },
    });
};

export const useModifyDivision = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: ({
            id,
            division,
        }: {
            id: number;
            division: DivisionData;
        }) => DivisionService.update(id, division),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["divisions"] });
        },
    });
};

export const useDeleteDivision = () => {
    const QueryClient = useQueryClient();

    return useMutation({
        mutationFn: (id: number) => DivisionService.delete(id),
        onSuccess: () => {
            QueryClient.invalidateQueries({ queryKey: ["divisions"] });
        },
    });
};
