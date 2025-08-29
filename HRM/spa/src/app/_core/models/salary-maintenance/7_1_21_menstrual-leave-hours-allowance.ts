export interface MenstrualLeaveHoursAllowanceParam {
    factory: string;
    permission_Group: string[];
    year_Month: string;
    employee_ID: string;
    is_Delete: boolean;
}

export interface MenstrualLeaveHoursAllowanceSource {
    param : MenstrualLeaveHoursAllowanceParam;
    totalRows: number;
}
