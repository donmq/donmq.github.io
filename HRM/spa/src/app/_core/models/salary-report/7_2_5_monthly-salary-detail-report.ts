export interface MonthlySalaryDetailReportParam {
  factory: string;
  year_Month: string;
  year_Month_Str: string;
  kind: string;
  permission_Group: string[];
  transfer: string;
  department: string;
  employee_Id: string;
  lang: string;
}

export interface MonthlySalaryDetailReportSource {
  param: MonthlySalaryDetailReportParam;
  totalRow: number;
}
