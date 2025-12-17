import { type JSX } from "react";
import { useForm } from "react-hook-form";
import { AxiosError } from "axios";
import type { CreateRefereeInput } from "../../../types/interfaces";
import { useCreateReferee } from "../../../hooks/RefereeHook";

function AddReferee(): JSX.Element {
    const createReferee = useCreateReferee();

    const { register, handleSubmit } = useForm<CreateRefereeInput>();

    async function onSubmit(data: CreateRefereeInput) {
        try {
            //console.log(data);
            await createReferee.mutateAsync(data);
        } catch (error) {
            if (error instanceof AxiosError) {
                console.log(error);
            }
        }
    }

    return (
        <div>
            <form onSubmit={handleSubmit(onSubmit)}>
                <label>
                    Bíró neve
                    <input
                        type="text"
                        {...register("name", {
                            required: "A bíró nevének megadása kötelező",
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
                    Születési idő
                    <input
                        type="date"
                        {...register("birth_date", {
                            required:
                                "A bíró születési dátumának megadása kötelező",
                        })}
                    />
                </label>
                <br />
                <button type="submit">Bíró hozzáadása</button>
            </form>
        </div>
    );
}

export default AddReferee;
