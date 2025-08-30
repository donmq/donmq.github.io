export interface MonthlyUnionDuesSummaryParam {
    factory: string;
    year_Month: string;
    department: string;
    userName: string;
    language: string;
}

export interface MonthlyUnionDuesSummarySource {
  param: MonthlyUnionDuesSummaryParam;
  totalRows: number;
}

