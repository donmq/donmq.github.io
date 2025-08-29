export interface EmployeeOvertimeDataSheet {
    factory: string;
    useR_GUID: string;
    department_Code: string;
    department_Name: string;
    employee_ID: string;
    local_Full_Name: string;
    work_Shift_Type: string;
    work_Shift_Type_Name: string;
    position_Title: string;
    position_Title_Name: string;
    work_Type: string;
    overtime_Date: string;
    work_Hours: number;
    clock_In: string;
    clock_Out: string;
    overtime_Hour: number;
    training_Hours: number;
    night_Hours: number;
    night_Overtime_Hours: number;
    holiday: string;
    last_Clock_In_Time: string;
}

export interface EmployeeOvertimeDataSheetParam {
    factory: string;
    employee_ID: string;
    department: string;
    language: string;
    overtime_Date_Start: string;
    overtime_Date_End: string;
    userName: string;
}