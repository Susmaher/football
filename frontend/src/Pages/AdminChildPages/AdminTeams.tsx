import { useState, type JSX } from "react";
import AddTeam from "../../components/AdminComponents/teamcomp/AddTeam";
import ModifyTeam from "../../components/AdminComponents/teamcomp/ModifyTeam";

function AdminTeams(): JSX.Element {
    const [selectedForm, setSelectedForm] = useState<string | null>(null);

    const handleClick = (form: string) => {
        if (form == "add") {
            setSelectedForm("add");
        } else {
            setSelectedForm("modify");
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

            {selectedForm == "modify" && <ModifyTeam />}
            {selectedForm == "add" && <AddTeam />}
        </>
    );
}
export default AdminTeams;
