import { useEffect, useState, type JSX } from "react";
import type {
    DivisionData,
    FieldData,
    ModifyTeamInputs,
    TeamData,
} from "../../../types/interfaces";
import { useForm } from "react-hook-form";
import { useFields } from "../../../hooks/FieldHook";
import { useDivisions } from "../../../hooks/DivisionHook";
import { useModifyTeam, useTeams } from "../../../hooks/TeamHook";
import { AxiosError } from "axios";

function ModifyTeam(): JSX.Element {
    const { data: fields } = useFields();
    const { data: divisions } = useDivisions();
    const { data: teams, isLoading } = useTeams();
    const [selectedTeam, setSelectedTeam] = useState<TeamData | undefined>(
        undefined
    );
    const { register, handleSubmit, reset } = useForm<ModifyTeamInputs>();

    const modifyTeam = useModifyTeam();

    async function onSubmit(data: ModifyTeamInputs) {
        if (!selectedTeam) return;

        try {
            //console.log("Route id: ", Number(selectedTeam.id));
            //console.log(data);
            await modifyTeam.mutateAsync({
                id: Number(selectedTeam.id),
                team: data,
            });
        } catch (error) {
            if (error instanceof AxiosError) {
                console.log(error);
            }
        }
    }

    useEffect(() => {
        if (selectedTeam) {
            reset({
                divisionId: Number(selectedTeam.divisionId),
                fieldId: Number(selectedTeam.fieldId),
                name: selectedTeam.name,
                points: Number(selectedTeam.points),
            });
        }
    }, [selectedTeam, reset]);

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
                                setSelectedTeam(team);
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
                {selectedTeam && (
                    <>
                        <label>
                            Válassz osztályt:
                            {divisions ? (
                                <select
                                    {...register("divisionId", {
                                        required:
                                            "Az osztály megadása kötelező",
                                    })}
                                    defaultValue={selectedTeam.divisionId}
                                >
                                    {divisions.map((division: DivisionData) => (
                                        <option
                                            key={division.id}
                                            value={division.id}
                                        >
                                            {division.name}
                                        </option>
                                    ))}
                                </select>
                            ) : (
                                <p>Loading...</p>
                            )}
                        </label>
                        <br />

                        <label>
                            Válassz pályát:
                            {fields ? (
                                <select
                                    {...register("fieldId", {
                                        required: "A pálya megadása kötelező ",
                                    })}
                                    defaultValue={selectedTeam.fieldId}
                                >
                                    {fields.map((field: FieldData) => (
                                        <option key={field.id} value={field.id}>
                                            {field.name}
                                        </option>
                                    ))}
                                </select>
                            ) : (
                                <p>Loading...</p>
                            )}
                        </label>
                        <br />

                        <label>
                            Csapat neve
                            <input
                                type="text"
                                defaultValue={selectedTeam.name}
                                {...register("name", {
                                    required:
                                        "A csapat nevének megadása kötelező",
                                    minLength: {
                                        value: 2,
                                        message:
                                            "A csapat nevének legalább 2 karakter hosszúnak kell lennie",
                                    },
                                })}
                            />
                        </label>
                        <br />

                        <label>
                            Pontok lacikám, pontok
                            <input
                                type="number"
                                defaultValue={selectedTeam.points}
                                {...register("points", {
                                    required: "A pontok megadása kötelező",
                                    min: {
                                        value: 0,
                                        message:
                                            "A csapat pontjának legalább 0-nak kell lennie",
                                    },
                                })}
                            />
                        </label>
                    </>
                )}

                <button type="submit">Csapat módosítása</button>
            </form>
        </div>
    );
}

export default ModifyTeam;
