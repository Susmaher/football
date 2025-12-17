import { useEffect, useState, type JSX } from "react";
import { useForm } from "react-hook-form";
import type { PositionData } from "../../../types/interfaces";
import { AxiosError } from "axios";
import { useModifyPosition, usePositions } from "../../../hooks/PositionHook";

function ModifyPosition(): JSX.Element {
    const { data: positions, isLoading } = usePositions();
    const [selectedPosition, setSelectedPosition] = useState<
        PositionData | undefined
    >(undefined);

    const { register, handleSubmit, reset } = useForm<PositionData>();

    const modifyPosition = useModifyPosition();

    async function onSubmit(data: PositionData) {
        if (!selectedPosition) return;

        try {
            //console.log(data);
            await modifyPosition.mutateAsync({
                id: Number(selectedPosition.id),
                position: data,
            });
        } catch (error) {
            if (error instanceof AxiosError) {
                console.log(error);
            }
        }
    }

    useEffect(() => {
        if (selectedPosition) {
            reset({
                id: selectedPosition.id,
                name: selectedPosition.name,
            });
        }
    }, [selectedPosition, reset]);

    if (isLoading) return <div>Loading...</div>;

    return (
        <div>
            <form onSubmit={handleSubmit(onSubmit)}>
                <label>
                    Válassz pozíciót:
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
                                setSelectedPosition(position);
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
                {selectedPosition && (
                    <>
                        <label>
                            Pálya neve
                            <input
                                type="text"
                                defaultValue={selectedPosition.name}
                                {...register("name", {
                                    required:
                                        "A pozíció nevének megadása kötelező",
                                    minLength: {
                                        value: 1,
                                        message:
                                            "A pozíció nevének legalább 1 karakter hosszúnak kell lennie",
                                    },
                                })}
                            />
                        </label>
                    </>
                )}

                <button type="submit">Pozíció módosítása</button>
            </form>
        </div>
    );
}

export default ModifyPosition;
