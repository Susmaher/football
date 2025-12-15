import { useEffect, useState, type JSX } from "react";
import { useDivisions, useModifyDivision } from "../../../hooks/DivisionHook";
import { useForm } from "react-hook-form";
import type { DivisionData } from "../../../types/interfaces";
import { AxiosError } from "axios";

function ModifyDivision(): JSX.Element {
    const { data: divisions, isLoading } = useDivisions();
    const [selectedDivision, setSelectedDivision] = useState<
        DivisionData | undefined
    >(undefined);

    const { register, handleSubmit, reset } = useForm<DivisionData>();

    const modifyDivision = useModifyDivision();

    async function onSubmit(data: DivisionData) {
        if (!selectedDivision) return;

        try {
            console.log(data);
            await modifyDivision.mutateAsync({
                id: Number(selectedDivision.id),
                division: data,
            });
        } catch (error) {
            if (error instanceof AxiosError) {
                console.log(error);
            }
        }
    }

    useEffect(() => {
        if (selectedDivision) {
            reset({
                id: selectedDivision.id,
                name: selectedDivision.name,
            });
        }
    }, [selectedDivision, reset]);

    if (isLoading) return <div>Loading...</div>;

    return (
        <div>
            <form onSubmit={handleSubmit(onSubmit)}>
                <label>
                    Válassz csapatot:
                    {divisions ? (
                        <select
                            {...register("id", {
                                required: "Az osztály megadása kötelező",
                            })}
                            defaultValue=""
                            onChange={(e) => {
                                const divisionId = e.target.value;
                                const division = divisions.find(
                                    (d) => d.id == divisionId
                                );
                                setSelectedDivision(division);
                                console.log(division);
                            }}
                        >
                            <option value="" disabled>
                                -- Válassz osztályt --
                            </option>
                            {divisions.map((division: DivisionData) => (
                                <option key={division.id} value={division.id}>
                                    {division.name}
                                </option>
                            ))}
                        </select>
                    ) : (
                        <p>Loading...</p>
                    )}
                </label>
                <br />
                {selectedDivision && (
                    <>
                        <label>
                            Osztály neve
                            <input
                                type="text"
                                defaultValue={selectedDivision.name}
                                {...register("name", {
                                    required:
                                        "Az osztály nevének megadása kötelező",
                                    minLength: {
                                        value: 2,
                                        message:
                                            "Az osztály nevének legalább 2 karakter hosszúnak kell lennie",
                                    },
                                })}
                            />
                        </label>
                    </>
                )}

                <button type="submit">Osztály módosítása</button>
            </form>
        </div>
    );
}

export default ModifyDivision;
