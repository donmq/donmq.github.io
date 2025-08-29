export interface D_7_2_6_MonthlyNonTransferSalaryPaymentReportParam {
  factory: string;
  year_Month: string;
  permission_Group: string[];
  department: string;
  employee_ID: string;
  language: string;
}

export interface MonthlyNonTransferSalaryPaymentReportSource {
  param: D_7_2_6_MonthlyNonTransferSalaryPaymentReportParam;
  totalRows: number;
  year_Month: Date;
}
