import type { JSX } from "react";
import type { CreatePositionInput } from "../../../types/interfaces";
import { AxiosError } from "axios";
import { useForm } from "react-hook-form";
import { useCreatePosition } from "../../../hooks/PositionHook";

function AddPosition(): JSX.Element {
    const createPosition = useCreatePosition();

    const { register, handleSubmit } = useForm<CreatePositionInput>();

    async function onSubmit(data: CreatePositionInput) {
        try {
            //console.log(data);
            await createPosition.mutateAsync(data);
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
                    Pozíció neve
                    <input
                        type="text"
                        {...register("name", {
                            required: "A pozíció nevének megadása kötelező",
                            minLength: {
                                value: 1,
                                message:
                                    "A pozíció nevének legalább 1 karakter hosszúnak kell lennie",
                            },
                        })}
                    />
                </label>
                <button type="submit">Pozíció hozzáadása</button>
            </form>
        </div>
    );
}

export default AddPosition;
