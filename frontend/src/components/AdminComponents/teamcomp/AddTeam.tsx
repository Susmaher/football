import { type JSX } from "react";
import { useForm } from "react-hook-form";
import { AxiosError } from "axios";
import { useFields } from "../../../hooks/FieldHook";
import { useDivisions } from "../../../hooks/DivisionHook";
import { useCreateTeam } from "../../../hooks/TeamHook";
import type {
    CreateTeamInputs,
    DivisionData,
    FieldData,
} from "../../../types/interfaces";

function AddTeam(): JSX.Element {
    const { data: fields } = useFields();
    const { data: divisions, isLoading } = useDivisions();

    const createTeam = useCreateTeam();

    const { register, handleSubmit } = useForm<CreateTeamInputs>();

    async function onSubmit(data: CreateTeamInputs) {
        try {
            //console.log(data);
            await createTeam.mutateAsync(data);
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
                <button type="submit">Csapat hozzáadása</button>
            </form>
        </div>
    );
}

export default AddTeam;
