export interface MonthlySalarySummaryReportParam {
  factory: string;
  year_Month: string;
  year_Month_Str: string;
  kind: string;
  permission_Group: string[];
  transfer: string;
  report_Kind: string;
  level: string;
  department: string;
  lang: string;
}

export interface MonthlySalarySummaryReportSource {
  param: MonthlySalarySummaryReportParam;
  totalRow: number;
}
