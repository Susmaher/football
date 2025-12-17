import type { CreateTeamInputs, ModifyTeamInputs } from "./../types/interfaces";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import TeamService from "../services/teamService";

export const useTeams = (divisionId?: number) => {
    return useQuery({
        queryKey: ["teams", divisionId],
        queryFn: async () => {
            const response = await TeamService.getAll(divisionId);
            return response.data;
        },
    });
};

export const useCreateTeam = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (team: CreateTeamInputs) => TeamService.create(team),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["teams"] });
        },
    });
};

export const useModifyTeam = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: ({ id, team }: { id: number; team: ModifyTeamInputs }) =>
            TeamService.update(id, team),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["teams"] });
        },
    });
};

export const useDeleteTeam = () => {
    const QueryClient = useQueryClient();

    return useMutation({
        mutationFn: (id: number) => TeamService.delete(id),
        onSuccess: () => {
            QueryClient.invalidateQueries({ queryKey: ["teams"] });
        },
    });
};
