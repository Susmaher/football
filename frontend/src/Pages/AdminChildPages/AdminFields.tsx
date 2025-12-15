import { useState, type JSX } from "react";
import ModifyField from "../../components/AdminComponents/fieldcomp/ModifyField";
import AddField from "../../components/AdminComponents/fieldcomp/AddField";
import DeleteField from "../../components/AdminComponents/fieldcomp/DeleteField";

function AdminFields(): JSX.Element {
    const [selectedForm, setSelectedForm] = useState<string | null>(null);

    const handleClick = (form: string) => {
        if (form == "add") {
            setSelectedForm("add");
        } else if (form == "modify") {
            setSelectedForm("modify");
        } else {
            setSelectedForm("delete");
        }
    };

    return (
        <>
            <h1>
                Ez itt a pályák kiállitása és szerkesztésének lehetősége kérem
                szépen
            </h1>

            <button onClick={() => handleClick("add")}>Pálya hozzáadása</button>
            <button onClick={() => handleClick("modify")}>
                Pálya módosítása
            </button>
            <button onClick={() => handleClick("delete")}>Pálya törlése</button>

            {selectedForm == "modify" && <ModifyField />}
            {selectedForm == "add" && <AddField />}
            {selectedForm == "delete" && <DeleteField />}
        </>
    );
}
export default AdminFields;
