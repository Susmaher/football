import { type JSX } from "react";
import { useForm } from "react-hook-form";
import { AxiosError } from "axios";
import { useTeams } from "../../../hooks/TeamHook";
import type {
    CreateTeamPlayerInput,
    PlayerData,
    TeamData,
} from "../../../types/interfaces";
import { usePlayers } from "../../../hooks/PlayerHook";
import { useCreateTeamPlayer } from "../../../hooks/TeamPlayerHook";

function AddTeamPlayer(): JSX.Element {
    const { data: teams } = useTeams();
    const { data: players, isLoading } = usePlayers();

    const createTeamPlayer = useCreateTeamPlayer();

    const { register, handleSubmit } = useForm<CreateTeamPlayerInput>();

    async function onSubmit(data: CreateTeamPlayerInput) {
        try {
            //console.log(data);
            await createTeamPlayer.mutateAsync(data);
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
                            {...register("teamId", {
                                required: "A csapat megadása kötelező",
                            })}
                            defaultValue=""
                        >
                            <option value="" disabled>
                                -- Válassz csapatot --
                            </option>
                            {teams.map((team: TeamData) => (
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
                <label>
                    Válassz játékost:
                    {players ? (
                        <select
                            {...register("playerId", {
                                required: "A játékos megadása kötelező ",
                            })}
                            defaultValue=""
                        >
                            <option value="" disabled>
                                -- Válassz játékost --
                            </option>
                            {players.map((player: PlayerData) => (
                                <option key={player.id} value={player.id}>
                                    {player.name} -{" "}
                                    {new Date(
                                        player.birth_date
                                    ).toLocaleDateString()}
                                </option>
                            ))}
                        </select>
                    ) : (
                        <p>Loading...</p>
                    )}
                </label>
                <br />
                <button type="submit">Játékos csapathoz rendelése</button>
            </form>
        </div>
    );
}

export default AddTeamPlayer;
