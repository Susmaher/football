import { useEffect, useState, type JSX } from "react";
import api from "../services/api";

interface MatchData {
    id: string;
    match_date: string;
    home_score: string | null;
    away_score: string | null;
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

function Matches(): JSX.Element {
    const [matches, setMatches] = useState<MatchData[]>([]);

    const fetchMatches = async () => {
        const response = await api.get("Matches");
        //console.log(response.data);
        setMatches(response.data);
    };

    useEffect(() => {
        const fetchData = async () => {
            await fetchMatches();
        };
        fetchData();
    }, []);

    const handleClick = async (status: string) => {
        const response = await api.get(`Matches?played=${status}`);
        console.log(response.data);
    };

    return (
        <>
            <h1>Matches</h1>
            {matches ? (
                matches.map((match: MatchData) => (
                    <div key={match.id}>
                        <h2>Dátum: {match.match_date}</h2>
                        <p>
                            Hazai csapat: {match.homeTeamName}; Vendég csapat:{" "}
                            {match.awayTeamName}
                        </p>
                        {match.home_score && (
                            <p>
                                Hazai gólok: {match.home_score}; Vendég gólok:{" "}
                                {match.away_score}
                            </p>
                        )}
                        <p>Osztály: {match.divisionName}</p>
                        <p>Kör: {match.round}</p>
                        {match.refereeName && <p>Bíró: {match.refereeName}</p>}
                        {match.fieldName && <p>Pálya: {match.fieldName}</p>}
                        <p>{match.status}</p>
                    </div>
                ))
            ) : (
                <p>Loading...</p>
            )}

            <button
                onClick={() => {
                    handleClick("true");
                }}
            >
                Played Matches
            </button>
        </>
    );
}

export default Matches;
