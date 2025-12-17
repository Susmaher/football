import type { JSX } from "react";
import { useForm } from "react-hook-form";
import type { DeleteInput, PositionData } from "../../../types/interfaces";
import { AxiosError } from "axios";
import { useDeletePosition, usePositions } from "../../../hooks/PositionHook";

function DeletePosition(): JSX.Element {
    const { data: positions, isLoading } = usePositions();

    const deletePosition = useDeletePosition();

    const { register, handleSubmit } = useForm<DeleteInput>();

    async function onSubmit(data: DeleteInput) {
        try {
            //console.log(data);
            await deletePosition.mutateAsync(data.id);
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
                    Válassz pályát:
                    {positions ? (
                        <select
                            {...register("id", {
                                required: "A pozíció megadása kötelező",
                            })}
                            defaultValue=""
                            onChange={(e) => {
                                const positionId = e.target.value;
                                const position = positions.find(
                                    (p) => p.id == positionId
                                );
                                console.log(position);
                            }}
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
                <button type="submit">Pozíció törlése</button>
            </form>
        </div>
    );
}

export default DeletePosition;
