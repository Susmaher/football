import { useState, type JSX } from "react";
import api from "../../services/api";

interface TeamData {
    id: string;
    name: string;
    points: string;
    divisionId: string;
    divisionName: string;
    fieldId: string;
    fieldName: string;
}

//needed fields and divisions

function AdminTeams(): JSX.Element {
    return (
        <>
            <h1>
                Ez itt a csapatok kiállitása és szerkesztésének lehetősége kérem
                szépen
            </h1>
            <button>Csapat hozzáadása</button>
            <button>Csapat módosítása</button>
        </>
    );
}
export default AdminTeams;
