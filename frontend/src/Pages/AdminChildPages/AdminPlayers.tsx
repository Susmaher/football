import { useState, type JSX } from "react";
import AddPlayer from "../../components/AdminComponents/playercomp/AddPlayer";
import ModifyPlayer from "../../components/AdminComponents/playercomp/ModifyPlayer";
import DeletePlayer from "../../components/AdminComponents/playercomp/DeletePlayer";
import AddTeamPlayer from "../../components/AdminComponents/playercomp/AddTeamPlayer";

function AdminPlayers(): JSX.Element {
    const [selectedForm, setSelectedForm] = useState<string | null>(null);

    const handleClick = (form: string) => {
        switch (form) {
            case "add":
                setSelectedForm("add");
                break;
            case "modify":
                setSelectedForm("modify");
                break;
            case "delete":
                setSelectedForm("delete");
                break;
            case "addteamp":
                setSelectedForm("addteamp");
                break;
        }
    };

    return (
        <>
            <h3>
                Ez itt a játékosok kiállitása és szerkesztésének lehetősége
                kérem szépen
            </h3>

            <button onClick={() => handleClick("add")}>
                Játékos hozzáadása
            </button>
            <button onClick={() => handleClick("modify")}>
                Játékos módosítása
            </button>
            <button onClick={() => handleClick("delete")}>
                Játékos törlése
            </button>
            <button onClick={() => handleClick("addteamp")}>
                Játékos csapathoz rendelése
            </button>

            {selectedForm == "modify" && <ModifyPlayer />}
            {selectedForm == "add" && <AddPlayer />}
            {selectedForm == "delete" && <DeletePlayer />}
            {selectedForm == "addteamp" && <AddTeamPlayer />}
        </>
    );
}
export default AdminPlayers;
