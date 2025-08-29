
export interface OvertimeHoursReportParam {
    factory: string;
    department: string;
    employee_Id: string;
    work_Shift_Type: string;
    kind: string;
    permission_Group: string[];
    date_From: string;
    date_To: string;
    lang: string;
}

export interface OvertimeHoursReport {
    factory: string;
    department: string;
    employee_Id: string;
    work_Shift_Type: string;
    kind: string;
    permission_Group: string;
    department_Name: string;
    local_Full_Name: string;
    trainingHours: number;
    regularOvertime: number;
    holidayOvertime: number;
    nightHours: number;
    nightOvertimeHour: number;
    positionTitle: string;
    overtimeHours: number;
    workingHours: number;
}
export interface OvertimeHoursReportMemory {
    params: OvertimeHoursReportParam;
    datas: OvertimeHoursReport[];
}