import type { JSX } from "react";
import { useDeleteDivision, useDivisions } from "../../../hooks/DivisionHook";
import { useForm } from "react-hook-form";
import type { DeleteInput, DivisionData } from "../../../types/interfaces";
import { AxiosError } from "axios";

function DeleteDivision(): JSX.Element {
    const { data: divisions, isLoading } = useDivisions();

    const deleteDivision = useDeleteDivision();

    const { register, handleSubmit } = useForm<DeleteInput>();

    async function onSubmit(data: DeleteInput) {
        try {
            console.log(data);
            await deleteDivision.mutateAsync(data.id);
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
                            {...register("id", {
                                required: "Az osztály megadása kötelező",
                            })}
                            defaultValue=""
                            onChange={(e) => {
                                const divisionId = e.target.value;
                                const division = divisions.find(
                                    (d) => d.id == divisionId
                                );
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
                <button type="submit">Osztály törlése</button>
            </form>
        </div>
    );
}

export default DeleteDivision;
