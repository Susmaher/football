import { useEffect, useState, type JSX } from "react";
import api from "../../services/api";

interface MatchData {
    match_date: string;
    round: string;
    status: string;
    homeTeamId: string;
    homeTeamName: string;
    awayTeamId: string;
    awayTeamName: string;
    divisionId: string;
    divisionName: string;
    refereeName: string | null;
    fieldName: string | null;
}

interface DivisionData {
    id: string;
    name: string;
}

function Draw(): JSX.Element {
    const [matches, setMatches] = useState<MatchData[]>([]);
    const [divisions, setDivisions] = useState<DivisionData[]>([]);
    const [selectedDivision, setSelectedDivision] = useState<string>("1");

    const fetchMatches = async (division: string) => {
        const response = await api.get(`Matches/draw/preview/${division}`);
        setMatches(response.data);
        if (response.data.length == 0) {
            alert("Ehhez az osztályhoz nem tartoznak csapatok");
        }
        //console.log(response.data);
    };

    const fetchDivisions = async () => {
        const response = await api.get("Divisions");
        //console.log(response.data);
        setDivisions(response.data);
    };

    useEffect(() => {
        const fetchData = async () => {
            await fetchDivisions();
        };
        fetchData();
    }, []);

    const handleClick = async (division: string) => {
        await fetchMatches(division);
    };

    return (
        <>
            <h1>Ez itt a sorsolás kérem szépen</h1>
            <label>
                Válassz osztály:
                {divisions ? (
                    <select
                        value={selectedDivision}
                        onChange={(e) => setSelectedDivision(e.target.value)}
                        id="division-pick"
                    >
                        {divisions.map((division) => (
                            <option key={division.id} value={division.id}>
                                {division.name}
                            </option>
                        ))}
                    </select>
                ) : (
                    <p>Loading...</p>
                )}
            </label>

            <button onClick={() => handleClick(selectedDivision)}>
                Sorsolás
            </button>
            {matches.length != 0 && (
                <div>
                    <h3>Divízió: {selectedDivision}</h3>
                    <table>
                        <thead>
                            <tr>
                                <th>Kör</th>
                                <th>Hazai csapat</th>
                                <th>Vendég csapat</th>
                                <th>Bíró</th>
                                <th>Pálya</th>
                            </tr>
                        </thead>
                        {matches.map((match, i: number) => (
                            <tbody key={i}>
                                <tr>
                                    <td>{match.round}</td>
                                    <td>{match.homeTeamName}</td>
                                    <td>{match.awayTeamName}</td>
                                    <td>{match.refereeName}</td>
                                    <td>{match.fieldName}</td>
                                </tr>
                            </tbody>
                        ))}
                    </table>
                </div>
            )}
        </>
    );
}

export default Draw;
