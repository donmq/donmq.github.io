export interface AnnualOvertimeHoursReportParam {
    factory: string;
    year_Month: string;
    department: string;
    employee_ID: string;
    language: string;
}

export interface AnnualOvertimeHoursReportSource {
    param : AnnualOvertimeHoursReportParam;
    totalRows: number;
}