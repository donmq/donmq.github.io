export interface NightShiftSubsidyMaintenance_Param{
    factory: string,
    year_Month: string,
    permission: string[],
    employee_ID: string;
    is_Delete: boolean;
}
export interface NightShiftSubsidyMaintenanceSource{
    param : NightShiftSubsidyMaintenance_Param;
    processRecords: number;
}


