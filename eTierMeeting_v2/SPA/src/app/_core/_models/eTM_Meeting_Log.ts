export interface eTM_Meeting_Log {
    record_ID: string;
    tU_ID: string;
    data_Date: string;
    meeting_Start_Time: Date;
    meeting_End_Time: Date | null;
    record_Status: string;
    duration_Sec: number | null;
    insert_By: string;
    insert_Time: Date;
    update_By: string;
    update_Time: Date | null;
}