import { useState, type JSX } from "react";
import ModifyReferee from "../../components/AdminComponents/refereecomp/ModifyReferee";
import AddReferee from "../../components/AdminComponents/refereecomp/AddReferee";
import DeleteReferee from "../../components/AdminComponents/refereecomp/DeleteReferee";

function AdminReferees(): JSX.Element {
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
                Ez itt a bírók kiállitása és szerkesztésének lehetősége kérem
                szépen
            </h3>

            <button onClick={() => handleClick("add")}>Bíró hozzáadása</button>
            <button onClick={() => handleClick("modify")}>
                Bíró módosítása
            </button>
            <button onClick={() => handleClick("delete")}>Bíró törlése</button>

            {selectedForm == "modify" && <ModifyReferee />}
            {selectedForm == "add" && <AddReferee />}
            {selectedForm == "delete" && <DeleteReferee />}
        </>
    );
}
export default AdminReferees;
