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

export interface CreateDivisionAndFieldData {
    name: string;
}

export interface FieldData {
    id: string;
    name: string;
}

export interface TeamData {
    id: string;
    name: string;
    points: string;
    divisionId: string;
    divisionName: string;
    fieldId: string;
    fieldName: string;
}

export interface CreateTeamInputs {
    name: string;
    divisionId: number;
    fieldId: number;
}

export interface ModifyTeamInputs {
    id: number;
    name: string;
    divisionId: number;
    fieldId: number;
    points: number;
}
