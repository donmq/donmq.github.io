export interface DepartmentMonthlyWorkingHoursReportParam {
    factory: string;
    language: string;
    yearMonth: string;
    permission_Group: string;
    permissionGroupTemp: string[];
}

export interface DepartmentMonthlyWorkingHoursReportMemory {
    param: DepartmentMonthlyWorkingHoursReportParam;
    totalRows: number;
}