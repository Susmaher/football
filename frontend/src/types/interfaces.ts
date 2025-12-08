export interface MatchData {
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

export interface DivisionData {
    id: string;
    name: string;
}

export interface FieldData {
    id: string;
    name: string;
}
