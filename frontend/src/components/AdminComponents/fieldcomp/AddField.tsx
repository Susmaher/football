import type { JSX } from "react";
import type { CreateDivisionAndFieldData } from "../../../types/interfaces";
import { AxiosError } from "axios";
import { useForm } from "react-hook-form";
import { useCreateField } from "../../../hooks/FieldHook";

function AddField(): JSX.Element {
    const createField = useCreateField();

    const { register, handleSubmit } = useForm<CreateDivisionAndFieldData>();

    async function onSubmit(data: CreateDivisionAndFieldData) {
        try {
            console.log(data);
            await createField.mutateAsync(data);
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
                    Pálya neve
                    <input
                        type="text"
                        {...register("name", {
                            required: "A pálya nevének megadása kötelező",
                            minLength: {
                                value: 2,
                                message:
                                    "A pálya nevének legalább 2 karakter hosszúnak kell lennie",
                            },
                        })}
                    />
                </label>
                <button type="submit">Pálya hozzáadása</button>
            </form>
        </div>
    );
}

export default AddField;
