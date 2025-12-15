import { useState, type JSX } from "react";
import AddTeam from "../../components/AdminComponents/teamcomp/AddTeam";
import ModifyTeam from "../../components/AdminComponents/teamcomp/ModifyTeam";
import DeleteTeam from "../../components/AdminComponents/teamcomp/DeleteTeam";

function AdminTeams(): JSX.Element {
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
                Ez itt a csapatok kiállitása és szerkesztésének lehetősége kérem
                szépen
            </h1>

            <button onClick={() => handleClick("add")}>
                Csapat hozzáadása
            </button>
            <button onClick={() => handleClick("modify")}>
                Csapat módosítása
            </button>
            <button onClick={() => handleClick("delete")}>
                Csapat törlése
            </button>

            {selectedForm == "modify" && <ModifyTeam />}
            {selectedForm == "add" && <AddTeam />}
            {selectedForm == "delete" && <DeleteTeam />}
        </>
    );
}
export default AdminTeams;
