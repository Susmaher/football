import { type JSX } from "react";
import { useForm } from "react-hook-form";
import { AxiosError } from "axios";
import type {
    CreatePlayerInput,
    PositionData,
} from "../../../types/interfaces";
import { usePositions } from "../../../hooks/PositionHook";
import { useCreatePlayer } from "../../../hooks/PlayerHook";

function AddPlayer(): JSX.Element {
    const { data: positions, isLoading } = usePositions();

    const createPlayer = useCreatePlayer();

    const { register, handleSubmit } = useForm<CreatePlayerInput>();

    async function onSubmit(data: CreatePlayerInput) {
        try {
            //console.log(data);
            await createPlayer.mutateAsync(data);
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
                    Válassz pozíciót:
                    {positions ? (
                        <select
                            {...register("positionId", {
                                required: "A pozíció megadása kötelező",
                            })}
                            defaultValue=""
                        >
                            <option value="" disabled>
                                -- Válassz pozíciót --
                            </option>
                            {positions.map((position: PositionData) => (
                                <option key={position.id} value={position.id}>
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
                        {...register("name", {
                            required: "A játékos nevének megadása kötelező",
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
                    Születési idő
                    <input
                        type="date"
                        {...register("birth_date", {
                            required:
                                "A játékos születési dátumának megadása kötelező",
                        })}
                    />
                </label>
                <br />
                <button type="submit">Játékos hozzáadása</button>
            </form>
        </div>
    );
}

export default AddPlayer;
