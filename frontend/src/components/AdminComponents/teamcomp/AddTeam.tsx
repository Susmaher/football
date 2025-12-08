import { useEffect, useState, type JSX } from "react";
import type { DivisionData, FieldData } from "../../../types/interfaces";
import api from "../../../services/api";
import { useForm } from "react-hook-form";
import { AxiosError } from "axios";

interface CreateTeamInputs {
    name: string;
    divisionId: number;
    fieldId: number;
}

function AddTeam(): JSX.Element {
    const [divisions, setDivisions] = useState<DivisionData[]>([]);
    const [fields, setFields] = useState<FieldData[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(false);

    const fetchFieldsAndDivisions = async () => {
        const divisionrespone = await api.get("Divisions");
        setDivisions(divisionrespone.data);
        const fieldrespone = await api.get("Fields");
        setFields(fieldrespone.data);
    };

    useEffect(() => {
        const fetchData = async () => {
            await fetchFieldsAndDivisions();
        };
        fetchData();
    }, []);

    const { register, handleSubmit, reset } = useForm<CreateTeamInputs>();

    async function createTeam(data: CreateTeamInputs) {
        try {
            setLoading(true);
            await api.post("Teams", {
                name: data.name,
                divisionId: data.divisionId,
                fieldId: data.fieldId,
            });
            reset();
        } catch (error) {
            if (error instanceof AxiosError) {
                setError(`Error: ${error.response?.data}`);
            }
        } finally {
            setLoading(false);
        }
    }

    return (
        <div>
            <form onSubmit={handleSubmit(createTeam)}>
                <label>
                    Válassz osztályt:
                    {divisions ? (
                        <select
                            {...register("divisionId", {
                                required: "Az osztály megadása kötelező",
                            })}
                            defaultValue=""
                        >
                            <option value="" disabled>
                                -- Válassz osztályt --
                            </option>
                            {divisions.map((division) => (
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
                <label>
                    Válassz pályát:
                    {fields ? (
                        <select
                            {...register("fieldId", {
                                required: "A pálya megadása kötelező ",
                            })}
                            defaultValue=""
                        >
                            <option value="" disabled>
                                -- Válassz pályát --
                            </option>
                            {fields.map((field) => (
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
                        {...register("name", {
                            required: "A csapat nevének megadása kötelező",
                            minLength: {
                                value: 2,
                                message:
                                    "A csapat nevének legalább 2 karakter hosszúnak kell lennie",
                            },
                        })}
                    />
                </label>
                {error && <span>{error}</span>}
                <button disabled={loading} type="submit">
                    Csapat hozzáadása
                </button>
            </form>
        </div>
    );
}

export default AddTeam;
