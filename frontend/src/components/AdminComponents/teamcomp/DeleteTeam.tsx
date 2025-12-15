import { type JSX } from "react";
import type { DeleteInput } from "../../../types/interfaces";
import { useDeleteTeam, useTeams } from "../../../hooks/TeamHook";
import { useForm } from "react-hook-form";
import { AxiosError } from "axios";

function DeleteTeam(): JSX.Element {
    const { data: teams, isLoading } = useTeams();

    const deleteTeam = useDeleteTeam();

    const { register, handleSubmit } = useForm<DeleteInput>();

    async function onSubmit(data: DeleteInput) {
        try {
            console.log(data);
            await deleteTeam.mutateAsync(data.id);
        } catch (error) {
            if (error instanceof AxiosError) {
                console.log(error);
            }
        }
    }

    if (isLoading) return <div>Loading...</div>;

    return (
        <div>
            <form onSubmit={handleSubmit(onSubmit)}>
                <label>
                    Válassz csapatot:
                    {teams ? (
                        <select
                            {...register("id", {
                                required: "A csapat megadása kötelező",
                            })}
                            defaultValue=""
                            onChange={(e) => {
                                const teamId = e.target.value;
                                const team = teams.find((t) => t.id == teamId);
                                console.log(team);
                            }}
                        >
                            <option value="" disabled>
                                -- Válassz csapatot --
                            </option>
                            {teams.map((team) => (
                                <option key={team.id} value={team.id}>
                                    {team.name}
                                </option>
                            ))}
                        </select>
                    ) : (
                        <p>Loading...</p>
                    )}
                </label>
                <br />
                <button type="submit">Csapat törlése</button>
            </form>
        </div>
    );
}

export default DeleteTeam;
