import { useEffect, useState, type JSX } from "react";
import { useForm } from "react-hook-form";
import type { DivisionData, FieldData } from "../../../types/interfaces";
import { AxiosError } from "axios";
import { useFields, useModifyField } from "../../../hooks/FieldHook";

function ModifyField(): JSX.Element {
    const { data: fields, isLoading } = useFields();
    const [selectedField, setSelectedField] = useState<FieldData | undefined>(
        undefined
    );

    const { register, handleSubmit, reset } = useForm<DivisionData>();

    const modifyField = useModifyField();

    async function onSubmit(data: FieldData) {
        if (!selectedField) return;

        try {
            //console.log(data);
            await modifyField.mutateAsync({
                id: Number(selectedField.id),
                field: data,
            });
        } catch (error) {
            if (error instanceof AxiosError) {
                console.log(error);
            }
        }
    }

    useEffect(() => {
        if (selectedField) {
            reset({
                id: selectedField.id,
                name: selectedField.name,
            });
        }
    }, [selectedField, reset]);

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
                                    (f) => f.id == fieldId
                                );
                                setSelectedField(field);
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
                {selectedField && (
                    <>
                        <label>
                            Pálya neve
                            <input
                                type="text"
                                defaultValue={selectedField.name}
                                {...register("name", {
                                    required:
                                        "A pálya nevének megadása kötelező",
                                    minLength: {
                                        value: 2,
                                        message:
                                            "A pálya nevének legalább 2 karakter hosszúnak kell lennie",
                                    },
                                })}
                            />
                        </label>
                    </>
                )}

                <button type="submit">Pálya módosítása</button>
            </form>
        </div>
    );
}

export default ModifyField;
