import { type JSX } from "react";
import type { DeleteInput } from "../../../types/interfaces";
import { useForm } from "react-hook-form";
import { AxiosError } from "axios";
import { useDeleteReferee, useReferees } from "../../../hooks/RefereeHook";

function DeleteReferee(): JSX.Element {
    const { data: referees, isLoading } = useReferees();

    const DeleteReferee = useDeleteReferee();

    const { register, handleSubmit } = useForm<DeleteInput>();

    async function onSubmit(data: DeleteInput) {
        try {
            //console.log(data);
            await DeleteReferee.mutateAsync(data.id);
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
                    Válassz játékost:
                    {referees ? (
                        <select
                            {...register("id", {
                                required: "A bíró megadása kötelező",
                            })}
                            defaultValue=""
                            onChange={(e) => {
                                const refereeId = e.target.value;
                                const referee = referees.find(
                                    (r) => r.id == refereeId
                                );
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
                <button type="submit">Bíró törlése</button>
            </form>
        </div>
    );
}

export default DeleteReferee;
