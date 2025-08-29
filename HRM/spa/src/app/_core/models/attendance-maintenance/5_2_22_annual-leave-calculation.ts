export interface AnnualLeaveCalculationParam {
    factory: string;
    start_Year_Month: string;
    end_Year_Month: string;
    kind: string;
    department: string;
    permission_Group: string [];
    language: string;
}

export interface AnnualLeaveCalculationSource {
    param : AnnualLeaveCalculationParam;
    totalRows: number;
}