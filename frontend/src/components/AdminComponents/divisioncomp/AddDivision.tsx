import type { JSX } from "react";
import { useCreateDivision } from "../../../hooks/DivisionHook";
import type { CreateDivisionAndFieldData } from "../../../types/interfaces";
import { AxiosError } from "axios";
import { useForm } from "react-hook-form";

function AddDivision(): JSX.Element {
    const createDivision = useCreateDivision();

    const { register, handleSubmit } = useForm<CreateDivisionAndFieldData>();

    async function onSubmit(data: CreateDivisionAndFieldData) {
        try {
            //console.log(data);
            await createDivision.mutateAsync(data);
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
                    Osztály neve
                    <input
                        type="text"
                        {...register("name", {
                            required: "Az osztály nevének megadása kötelező",
                            minLength: {
                                value: 2,
                                message:
                                    "Az osztály nevének legalább 2 karakter hosszúnak kell lennie",
                            },
                        })}
                    />
                </label>
                <button type="submit">Osztály hozzáadása</button>
            </form>
        </div>
    );
}

export default AddDivision;
