import { useEffect, useState, type JSX } from "react";
import api from "../../services/api";
import AddTeam from "../../components/AdminComponents/teamcomp/AddTeam";

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
    const [isVisible, setIsVisible] = useState<boolean>(false);
    return (
        <>
            <h1>
                Ez itt a csapatok kiállitása és szerkesztésének lehetősége kérem
                szépen
            </h1>

            {isVisible && <AddTeam />}

            <button onClick={() => setIsVisible(true)}>
                Csapat hozzáadása
            </button>
            <button>Csapat módosítása</button>
        </>
    );
}
export default AdminTeams;

/*<label>
                Válassz csapatot:
                {teams ? (
                    <select
                        value={selectedTeam}
                        onChange={(e) => setSelectedTeam(e.target.value)}
                        id="division-pick"
                    >
                        {teams.map((team) => (
                            <option key={team.id} value={team.id}>
                                {team.name}
                            </option>
                        ))}
                    </select>
                ) : (
                    <p>Loading...</p>
                )}
            </label>
            <br /> */
