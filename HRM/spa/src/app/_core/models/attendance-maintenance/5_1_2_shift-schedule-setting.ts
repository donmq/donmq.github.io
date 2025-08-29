import { BaseSource } from "../base-source";

export interface HRMS_Att_Work_ShiftDto {
    division: string;
    factory: string;
    work_Shift_Type: string;
    work_Shift_Type_Title: string;
    week: string;
    clock_In: string;
    clock_Out: string;
    overtime_ClockIn: string;
    overtime_ClockOut: string;
    lunch_Start: string;
    overnight: string;
    work_Hours: number;
    work_Days: number;
    effective_State: boolean;
    update_By: string;
    update_Time: string;
}

export interface HRMS_Att_Work_ShiftParam {
    division: string;
    factory: string;
    work_Shift_Type: string;
    language: string;
}

export interface HRMS_Att_Work_Shift {
    division: string;
    factory: string;
    work_Shift_Type: string;
    week: number;
    clock_In: string;
    clock_Out: string;
    overtime_ClockIn: string;
    overtime_ClockOut: string;
    lunch_Start: string;
    lunch_End: string;
    overnight: string;
    work_Hours: string;
    work_Days: string;
    effective_State: boolean;
    update_By: string;
    update_Time: Date;
    update_Time_Str: string;
}

export interface HRMS_Att_Work_ShiftParam {
    division: string;
    factory: string;
    language: string;
}

export interface HRMS_Att_Work_ShiftSource extends BaseSource<HRMS_Att_Work_Shift> {
    param: HRMS_Att_Work_ShiftParam,
}
