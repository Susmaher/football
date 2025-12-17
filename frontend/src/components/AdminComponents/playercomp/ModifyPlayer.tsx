import { useEffect, useState, type JSX } from "react";
import type {
    ModifyPlayerInput,
    PlayerData,
    PositionData,
} from "../../../types/interfaces";
import { useForm } from "react-hook-form";
import { AxiosError } from "axios";
import { usePositions } from "../../../hooks/PositionHook";
import { useModifyPlayer, usePlayers } from "../../../hooks/PlayerHook";

function ModifyPlayer(): JSX.Element {
    const { data: positions } = usePositions();
    const { data: players, isLoading } = usePlayers();
    const [selectedPlayer, setSelectedPlayer] = useState<
        PlayerData | undefined
    >(undefined);
    const { register, handleSubmit, reset } = useForm<ModifyPlayerInput>();

    const modifyPlayer = useModifyPlayer();

    async function onSubmit(data: ModifyPlayerInput) {
        if (!selectedPlayer) return;

        try {
            //console.log("Route id: ", Number(selectedTeam.id));
            //console.log(data);
            await modifyPlayer.mutateAsync({
                id: Number(selectedPlayer.id),
                player: data,
            });
        } catch (error) {
            if (error instanceof AxiosError) {
                console.log(error);
            }
        }
    }

    useEffect(() => {
        if (selectedPlayer) {
            reset({
                name: selectedPlayer.name,
                birth_date: selectedPlayer.birth_date,
                positionId: Number(selectedPlayer.positionId),
            });
        }
    }, [selectedPlayer, reset]);

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
                                    (t) => t.id == playerId
                                );
                                setSelectedPlayer(player);
                                console.log(player);
                            }}
                        >
                            <option value="" disabled>
                                -- Válassz játékos --
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
                {selectedPlayer && (
                    <>
                        <label>
                            Válassz pozíciót:
                            {positions ? (
                                <select
                                    {...register("positionId", {
                                        required:
                                            "Az osztály megadása kötelező",
                                    })}
                                    defaultValue={selectedPlayer.positionId}
                                >
                                    {positions.map((position: PositionData) => (
                                        <option
                                            key={position.id}
                                            value={position.id}
                                        >
                                            {position.name}
                                        </option>
                                    ))}
                                </select>
                            ) : (
                                <p>Loading...</p>
                            )}
                        </label>
                        <br />

                        <label>
                            Játékos neve
                            <input
                                type="text"
                                defaultValue={selectedPlayer.name}
                                {...register("name", {
                                    required:
                                        "A játékos nevének megadása kötelező",
                                    minLength: {
                                        value: 2,
                                        message:
                                            "A játékos nevének legalább 2 karakter hosszúnak kell lennie",
                                    },
                                })}
                            />
                        </label>
                        <br />

                        <label>
                            Születési dátum
                            <input
                                type="date"
                                defaultValue={new Date(
                                    selectedPlayer.birth_date
                                ).toLocaleDateString()}
                                {...register("birth_date", {
                                    required:
                                        "A játékos születési dátumának megadása kötelező",
                                })}
                            />
                        </label>
                    </>
                )}

                <button type="submit">Játékos módosítása</button>
            </form>
        </div>
    );
}

export default ModifyPlayer;
