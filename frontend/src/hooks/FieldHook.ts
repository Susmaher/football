import type {
    CreateDivisionAndFieldData,
    FieldData,
} from "./../types/interfaces";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import FieldService from "../services/fieldService";

export const useFields = () => {
    return useQuery({
        queryKey: ["fields"],
        queryFn: async () => {
            const response = await FieldService.getAll();
            return response.data;
        },
    });
};

export const useCreateField = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (field: CreateDivisionAndFieldData) =>
            FieldService.create(field),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["fields"] });
        },
    });
};

export const useModifyField = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: ({ id, field }: { id: number; field: FieldData }) =>
            FieldService.update(id, field),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["fields"] });
        },
    });
};

export const useDeleteField = () => {
    const QueryClient = useQueryClient();

    return useMutation({
        mutationFn: (id: number) => FieldService.delete(id),
        onSuccess: () => {
            QueryClient.invalidateQueries({ queryKey: ["fields"] });
        },
    });
};
