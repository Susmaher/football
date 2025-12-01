import { useEffect, useState, type JSX } from "react";
import api from "../services/api";

interface MatchData {
    id: string;
    Match_date: string;
    Home_score: string | null;
    Away_score: string | null;
    Round: string;
    Status: string;
    HomeTeamId: string;
    HomeTeamName: string;
    AwayTeamId: string;
    AwayTeamName: string;
    divisionId: string;
    divisionName: string;
    refereeName: string | null;
    fieldName: string | null;
}

function Matches(): JSX.Element {
    const [matches, setMatches] = useState<MatchData[]>([]);

    const fetchMatches = async () => {
        const response = await api.get("Matches");
        setMatches(response.data);
    };

    useEffect(() => {
        const fetchData = async () => {
            await fetchMatches();
        };
        fetchData();
    }, []);

    return (
        <>
            <h1>Matches</h1>
            {matches ? (
                matches.map((match: MatchData) => (
                    <div key={match.id}>
                        <h2>Dátum: {match.Match_date}</h2>
                        <p>
                            Hazai csapat: {match.HomeTeamName}; Vendég csapat:{" "}
                            {match.AwayTeamName}
                        </p>
                    </div>
                ))
            ) : (
                <p>Loading...</p>
            )}
        </>
    );
}

export default Matches;
