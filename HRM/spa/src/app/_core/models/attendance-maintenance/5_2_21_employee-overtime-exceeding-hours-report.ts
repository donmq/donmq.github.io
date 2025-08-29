export interface EmployeeOvertimeExceedingHoursReportParam {
    factory: string;
    start_Date: string;
    end_Date: string;
    statistical_Method: string;
    abnormal_Overtime_Hours: number;
    language: string;
}

export interface EmployeeOvertimeExceedingHoursReportSource {
    param : EmployeeOvertimeExceedingHoursReportParam;
    totalRows: number;
}