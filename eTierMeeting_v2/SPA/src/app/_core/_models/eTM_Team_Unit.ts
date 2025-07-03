export interface eTM_Team_Unit {
    tU_ID: string;
    tU_Name: string;
    center_Level: string;
    tier_Level: string;
    tU_Code: string;
    class1_Level: string;
    class2_Level: string;
    insert_By: string;
    insert_Time: Date;
    update_By: string;
    update_Time: Date | null;
}
export interface eTM_Team_UnitIndexOC {
    id: number;
    level: number;
    parentID: number | null;
    tU_ID: string;
    tU_Name: string;
    center_Level: string;
    tier_Level: string;
    tU_Code: string;
    class1_Level: string;
    class2_Level: string;
    sortSeq: number | null;
    lineNum: number;
    rowCount: number | null;
    building: string;
    isActive: boolean;
}