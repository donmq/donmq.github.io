export interface MonthlySalaryAdditionsDeductionsSummaryReportParam {
    factory: string;
    year_Month: string;
    permission_Group: string[];
    department: string;
    kind: string;
    language: string;
}

export interface MonthlySalaryAdditionsDeductionsSummaryReportSource {
    param : MonthlySalaryAdditionsDeductionsSummaryReportParam;
    totalRows: number;
}