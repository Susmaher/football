import { useState, type JSX } from "react";
import AddDivision from "../../components/AdminComponents/divisioncomp/AddDivision";
import ModifyDivision from "../../components/AdminComponents/divisioncomp/ModifyDivision";
import DeleteDivision from "../../components/AdminComponents/divisioncomp/DeleteDivision";

function AdminDivisions(): JSX.Element {
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
                Ez itt az osztályok kiállitása és szerkesztésének lehetősége
                kérem szépen
            </h1>

            <button onClick={() => handleClick("add")}>
                Osztály hozzáadása
            </button>
            <button onClick={() => handleClick("modify")}>
                Osztály módosítása
            </button>
            <button onClick={() => handleClick("delete")}>
                Osztály törlése
            </button>

            {selectedForm == "modify" && <ModifyDivision />}
            {selectedForm == "add" && <AddDivision />}
            {selectedForm == "delete" && <DeleteDivision />}
        </>
    );
}
export default AdminDivisions;
