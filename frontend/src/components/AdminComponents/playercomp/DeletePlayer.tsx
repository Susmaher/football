import { type JSX } from "react";
import type { DeleteInput } from "../../../types/interfaces";
import { useForm } from "react-hook-form";
import { AxiosError } from "axios";
import { useDeletePlayer, usePlayers } from "../../../hooks/PlayerHook";

function DeletePlayer(): JSX.Element {
    const { data: players, isLoading } = usePlayers();

    const deletePlayer = useDeletePlayer();

    const { register, handleSubmit } = useForm<DeleteInput>();

    async function onSubmit(data: DeleteInput) {
        try {
            //console.log(data);
            await deletePlayer.mutateAsync(data.id);
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
                    Válassz játékost:
                    {players ? (
                        <select
                            {...register("id", {
                                required: "A játékos megadása kötelező",
                            })}
                            defaultValue=""
                            onChange={(e) => {
                                const playerId = e.target.value;
                                const player = players.find(
                                    (p) => p.id == playerId
                                );
                                console.log(player);
                            }}
                        >
                            <option value="" disabled>
                                -- Válassz játékost --
                            </option>
                            {players.map((player) => (
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
                <button type="submit">Játékos törlése</button>
            </form>
        </div>
    );
}

export default DeletePlayer;
