export interface IndividualMonthlyWorkingHoursReportParam {
    factory: string;
    language: string;
    yearMonth: string;
    permission_Group: string;
    permissionGroupTemp: string[];
}

export interface IndividualMonthlyWorkingHoursReportMemory {
    param: IndividualMonthlyWorkingHoursReportParam;
    totalRows: number;
}