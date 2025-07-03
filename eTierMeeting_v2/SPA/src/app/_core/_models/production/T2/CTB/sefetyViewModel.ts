export interface SefetyViewModel {
    month: number;
    year: number;
    evaluetions: EvaluetionCategory[];
    lines: string[];
}

export interface EvaluetionCategory {
    item_ID: string;
    item_Name: string;
    item_Name_LL: string;
    target: string | null;
    achievements: Achievement[];
}

export interface Achievement {
    hsE_Score_ID: number;
    item_ID: string;
    line_Sname: string;
    score: string;
    isPass: boolean;
}