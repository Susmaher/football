import type { JSX } from "react";
import { useTeams } from "../hooks/TeamHook";
import type { TeamData } from "../types/interfaces";

function Teams(): JSX.Element {
    const { data: teams, isLoading, error } = useTeams(2);

    if (isLoading) return <div>Loading...</div>;
    if (error) return <div>Error: {error.message}</div>;

    return (
        <>
            <h1>Teams</h1>
            {teams!.map((team: TeamData) => (
                <div key={team.id}>
                    <h3>{team.name}</h3>
                    <p>Points: {team.points}</p>
                    <p>Division: {team.divisionName}</p>
                    <p>Field: {team.fieldName}</p>
                </div>
            ))}
        </>
    );
}

export default Teams;
