import { useEffect, useState, type JSX } from "react";
import api from "../services/api";

interface TeamData {
    id: string;
    name: string;
    points: string;
    divisionId: string;
    divisionName: string;
    fieldId: string;
    fieldName: string;
}

function Teams(): JSX.Element {
    const [teams, setTeams] = useState<TeamData[]>([]);

    const fetchTeams = async () => {
        const response = await api.get("Teams");
        setTeams(response.data);
    };

    useEffect(() => {
        const fetchData = async () => {
            await fetchTeams();
        };
        fetchData();
    }, []);

    const handleOnClick = async (divNum: number) => {
        const response = await api.get(`Teams?division=${divNum}`);
        console.log(response.data);
    };

    return (
        <>
            <h1>Teams</h1>
            {teams ? (
                teams.map((team: TeamData) => (
                    <div key={team.id}>
                        <h3>{team.name}</h3>
                        <p>Points: {team.points}</p>
                        <p>Division: {team.divisionName}</p>
                        <p>Field: {team.fieldName}</p>
                    </div>
                ))
            ) : (
                <p>Loading...</p>
            )}

            <button onClick={() => handleOnClick(1)}>
                Get Teams in division 2
            </button>
        </>
    );
}

export default Teams;
