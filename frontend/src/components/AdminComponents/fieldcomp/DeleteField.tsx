import type { JSX } from "react";
import { useForm } from "react-hook-form";
import type { DeleteInput, FieldData } from "../../../types/interfaces";
import { AxiosError } from "axios";
import { useDeleteField, useFields } from "../../../hooks/FieldHook";

function DeleteField(): JSX.Element {
    const { data: fields, isLoading } = useFields();

    const deleteField = useDeleteField();

    const { register, handleSubmit } = useForm<DeleteInput>();

    async function onSubmit(data: DeleteInput) {
        try {
            //console.log(data);
            await deleteField.mutateAsync(data.id);
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
                    Válassz pályát:
                    {fields ? (
                        <select
                            {...register("id", {
                                required: "A pálya megadása kötelező",
                            })}
                            defaultValue=""
                            onChange={(e) => {
                                const fieldId = e.target.value;
                                const field = fields.find(
                                    (d) => d.id == fieldId
                                );
                                console.log(field);
                            }}
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
                <button type="submit">Pálya törlése</button>
            </form>
        </div>
    );
}

export default DeleteField;
