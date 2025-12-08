import { useEffect, useState, type JSX } from "react";
import api from "../../services/api";
import type { DivisionData, FieldData } from "../../types/interfaces";

interface TeamData {
    id: string;
    name: string;
    points: string;
    divisionId: string;
    divisionName: string;
    fieldId: string;
    fieldName: string;
}

interface CreateTeam {
    name: string;
    divisionId: number;
    fieldId: number;
}

//needed fields and divisions

function AdminTeams(): JSX.Element {
    const [divisions, setDivisions] = useState<DivisionData[]>([]);
    const [fields, setFields] = useState<FieldData[]>([]);
    const [teams, setTeams] = useState<TeamData[]>([]);
    const [selectedDivision, setSelectedDivision] = useState("1");
    const [selectedField, setSelectedField] = useState("1");
    const [selectedTeam, setSelectedTeam] = useState("1");
    const [teamName, setTeamName] = useState("");

    const [isVisible, setIsVisible] = useState<boolean>(false);

    const fetchFieldsAndDivisions = async () => {
        const divisionrespone = await api.get("Divisions");
        setDivisions(divisionrespone.data);
        const fieldrespone = await api.get("Fields");
        setFields(fieldrespone.data);
        const teamresponse = await api.get("Teams");
        setTeams(teamresponse.data);
    };

    useEffect(() => {
        const fetchData = async () => {
            await fetchFieldsAndDivisions();
        };
        fetchData();
    }, []);

    const createTeamask = async () => {
        await api.post("Teams", {
            name: teamName,
            divisionId: selectedDivision,
            fieldId: selectedField,
        });
    };

    return (
        <>
            <h1>
                Ez itt a csapatok kiállitása és szerkesztésének lehetősége kérem
                szépen
            </h1>
            {isVisible && (
                <div>
                    <form onSubmit={createTeamask}>
                        <label>
                            Válassz osztályt:
                            {divisions ? (
                                <select
                                    value={selectedDivision}
                                    onChange={(e) =>
                                        setSelectedDivision(e.target.value)
                                    }
                                    id="division-pick"
                                >
                                    {divisions.map((division) => (
                                        <option
                                            key={division.id}
                                            value={division.id}
                                        >
                                            {division.name}
                                        </option>
                                    ))}
                                </select>
                            ) : (
                                <p>Loading...</p>
                            )}
                        </label>
                        <br />
                        <label>
                            Válassz pályát:
                            {fields ? (
                                <select
                                    value={selectedField}
                                    onChange={(e) =>
                                        setSelectedField(e.target.value)
                                    }
                                    id="division-pick"
                                >
                                    {fields.map((field) => (
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

                        <label>
                            Csapat neve
                            <input
                                type="text"
                                onChange={(e) => setTeamName(e.target.value)}
                            />
                        </label>
                        <button type="submit">Csapat hozzáadása</button>
                    </form>
                </div>
            )}

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
