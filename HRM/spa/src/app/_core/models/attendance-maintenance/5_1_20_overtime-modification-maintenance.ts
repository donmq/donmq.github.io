import { Pagination } from "@utilities/pagination-utility";

export interface OvertimeModificationMaintenanceParam {
    factory: string;
    department: string;
    employee_ID: string;
    work_Shift_Type: string;
    overtime_Date_From_Date: string;
    overtime_Date_From: string;
    overtime_Date_To_Date: string;
    overtime_Date_To: string;
    language: string;
    attDate: string;
}
export interface OvertimeModificationMaintenanceDto {
    useR_GUID: string;
    factory: string;
    overtime_Date: string;
    overtime_Date_Str: string;
    employee_ID: string;
    department_Code: string;
    department_Name: string;
    department_Code_Name: string;
    work_Shift_Type: string;
    work_Shift_Type_Name: string;
    overtime_Start: string;
    overtime_Start_Temp: string;
    overtime_End: string;
    overtime_End_Temp: string;
    overtime_Hours: string;
    night_Hours: string;
    night_Overtime_Hours: string;
    training_Hours: string;
    night_Eat_Times: string;
    holiday: string;
    holiday_Name: string;
    update_By: string;
    update_Time: Date | string | null;
    update_Time_Temp: string;
    local_Full_Name: string;
    work_Shift_Type_Time: string;
    clock_In_Time: string;
    clock_Out_Time: string;
    isOvertimeDate: boolean;
}

export interface EmpPersonalAdd {
    useR_GUID: string;
    local_Full_Name: string;
    division: string;
    factory: string;
    employee_ID: string;
    department: string;
    department_Name: string;
    assigned_Division: string;
    assigned_Factory: string;
    assigned_Employee_ID: string;
    assigned_Department: string;
    work_Shift_Type: string;
    work8hours: boolean | null;
    employment_Status: string;
}

export interface ParamMain5_20 {
    paramSearch: OvertimeModificationMaintenanceParam;
    data: OvertimeModificationMaintenanceDto[];
    pagination : Pagination;
}

export interface ClockInClockOut {
    work_Shift_Type_Time: string;
    clock_In_Time: string;
    clock_Out_Time: string;
}
