export interface Surrogate {
    userID: number;
    userRank: number | null;
    empID: number | null;
    fullName: string;
    empNumber: string;
    surrogateId: number;
}

export interface SurrogateParam {
    userID: number;
    partID: number;
    keyword: string;
}

export interface SurrogateRemove {
    userID: number;
    surrogateId: number;
}