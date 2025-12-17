import { useState, type JSX } from "react";
import ModifyPosition from "../../components/AdminComponents/positioncomp/ModifyPosition";
import AddPosition from "../../components/AdminComponents/positioncomp/AddPosition";
import DeletePosition from "../../components/AdminComponents/positioncomp/DeletePosition";

function AdminPositions(): JSX.Element {
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
            <h3>
                Ez itt a pozíciók kiállitása és szerkesztésének lehetősége kérem
                szépen
            </h3>

            <button onClick={() => handleClick("add")}>
                Pozíció hozzáadása
            </button>
            <button onClick={() => handleClick("modify")}>
                Pozíció módosítása
            </button>
            <button onClick={() => handleClick("delete")}>
                Pozíció törlése
            </button>

            {selectedForm == "modify" && <ModifyPosition />}
            {selectedForm == "add" && <AddPosition />}
            {selectedForm == "delete" && <DeletePosition />}
        </>
    );
}
export default AdminPositions;
