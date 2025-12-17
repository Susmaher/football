import { useEffect, useState, type JSX } from "react";
import type { RefereeData } from "../../../types/interfaces";
import { useForm } from "react-hook-form";
import { AxiosError } from "axios";
import { useModifyReferee, useReferees } from "../../../hooks/RefereeHook";

function ModifyReferee(): JSX.Element {
    const { data: referees, isLoading } = useReferees();
    const [selectedReferee, setSelectedReferee] = useState<
        RefereeData | undefined
    >(undefined);
    const { register, handleSubmit, reset } = useForm<RefereeData>();

    const modifyReferee = useModifyReferee();

    async function onSubmit(data: RefereeData) {
        if (!selectedReferee) return;

        try {
            //console.log("Route id: ", Number(selectedTeam.id));
            //console.log(data);
            await modifyReferee.mutateAsync({
                id: Number(selectedReferee.id),
                referee: data,
            });
        } catch (error) {
            if (error instanceof AxiosError) {
                console.log(error);
            }
        }
    }

    useEffect(() => {
        if (selectedReferee) {
            reset({
                name: selectedReferee.name,
                birth_date: selectedReferee.birth_date,
            });
        }
    }, [selectedReferee, reset]);

    if (isLoading) return <div>Loading...</div>;

    return (
        <div>
            <form onSubmit={handleSubmit(onSubmit)}>
                <label>
                    Válassz bírót:
                    {referees ? (
                        <select
                            {...register("id", {
                                required: "A játékos megadása kötelező",
                            })}
                            defaultValue=""
                            onChange={(e) => {
                                const refereeId = e.target.value;
                                const referee = referees.find(
                                    (r) => r.id == refereeId
                                );
                                setSelectedReferee(referee);
                                console.log(referee);
                            }}
                        >
                            <option value="" disabled>
                                -- Válassz bírót --
                            </option>
                            {referees.map((referee) => (
                                <option key={referee.id} value={referee.id}>
                                    {referee.name} -{" "}
                                    {new Date(
                                        referee.birth_date
                                    ).toLocaleDateString()}
                                </option>
                            ))}
                        </select>
                    ) : (
                        <p>Loading...</p>
                    )}
                </label>
                <br />
                {selectedReferee && (
                    <>
                        <label>
                            Bíró neve
                            <input
                                type="text"
                                defaultValue={selectedReferee.name}
                                {...register("name", {
                                    required:
                                        "A bíró nevének megadása kötelező",
                                    minLength: {
                                        value: 2,
                                        message:
                                            "A bíró nevének legalább 2 karakter hosszúnak kell lennie",
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
                                    selectedReferee.birth_date
                                ).toLocaleDateString()}
                                {...register("birth_date", {
                                    required:
                                        "A bíró születési dátumának megadása kötelező",
                                })}
                            />
                        </label>
                    </>
                )}

                <button type="submit">Bíró módosítása</button>
            </form>
        </div>
    );
}

export default ModifyReferee;
