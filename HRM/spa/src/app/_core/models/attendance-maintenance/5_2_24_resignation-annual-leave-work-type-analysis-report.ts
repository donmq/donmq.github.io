export interface ResignationAnnualLeaveWorktypeAnalysisReportParam {
    factory: string;
    dateStart: string;
    dateEnd: string;
    permission_Group: string[];
    level: string;
    lang: string;
}

export interface ResignationAnnualLeaveWorktypeAnalysisReportSource {
    param : ResignationAnnualLeaveWorktypeAnalysisReportParam;
    totalRows: number;
}