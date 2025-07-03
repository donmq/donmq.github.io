export interface HSEDataSearchDto {
    hsE_Score_ID: number;
    center_Level: string;
    tier_Level: string;
    class_Level: string;
    building: string;
    line_Sname: string;
    dept_ID: string;
    evaluation: string;
    score: number;
    target: number | null;
    update_By: string;
    update_Time: string | null;
    action: string;
    checkImageAlert: boolean;
}

export interface HSESearchParam {
    year: number;
    month: number;
    building: string;
    deptID: string;
    clickImageAlert: boolean;
}