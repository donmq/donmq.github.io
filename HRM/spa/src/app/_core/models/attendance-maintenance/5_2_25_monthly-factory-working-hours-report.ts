export interface MonthlyFactoryWorkingHoursReportParam {
    factory: string;
    start_Date: string;
    end_Date: string;
    permission_Group: string[];
    language: string;
}

export interface MonthlyFactoryWorkingHoursReportSource {
    param : MonthlyFactoryWorkingHoursReportParam;
    totalRows: number;
}