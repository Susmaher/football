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

//---------------------------------------------------
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

//---------------------------------------------------
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

//---------------------------------------------------
export interface DeleteInput {
    id: number;
}
//---------------------------------------------------
export interface CreatePositionInput {
    name: string;
}

export interface PositionData {
    id: string;
    name: string;
}

//---------------------------------------------------
export interface PlayerData {
    id: string;
    name: string;
    birth_date: Date;
    positionId: number;
    positionName: string;
}

export interface CreatePlayerInput {
    name: string;
    birth_date: Date;
    positionId: number;
}

export interface ModifyPlayerInput {
    id: string;
    name: string;
    birth_date: Date;
    positionId: number;
}

//---------------------------------------------------
export interface RefereeData {
    id: string;
    name: string;
    birth_date: Date;
}

export interface CreateRefereeInput {
    name: string;
    birth_date: Date;
}

//---------------------------------------------------
export interface TeamPlayerData {
    id: string;
    teamId: number;
    teamName: string;
    playerId: number;
    playerName: string;
}

export interface CreateTeamPlayerInput {
    teamId: number;
    playerId: number;
}

export interface ModifyTeamPlayerInput {
    id: string;
    teamId: number;
    playerId: number;
}
