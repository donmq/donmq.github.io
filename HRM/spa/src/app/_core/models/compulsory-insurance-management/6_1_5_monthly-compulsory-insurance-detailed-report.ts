export interface MonthlyCompulsoryInsuranceDetailedReportParam {
  factory: string;
  year_Month: string;
  permission_Group: string[];
  permission_Group_Name: string[];
  department: string;
  insurance_Type: string;
  insurance_Type_Full: string;
  kind: string;
  language: string;
}

export interface MonthlyCompulsoryInsuranceDetailedReportSource {
  param: MonthlyCompulsoryInsuranceDetailedReportParam;
  totalRows: number;
}
