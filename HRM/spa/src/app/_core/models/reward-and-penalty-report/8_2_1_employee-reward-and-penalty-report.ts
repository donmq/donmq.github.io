export interface EmployeeRewardAndPenaltyReportParam {
  factory: string;
  start_Year_Month: string;
  end_Year_Month: string;
  permission_Group: string[];
  department: string;
  employee_ID: string;
  start_Date: string;
  end_Date: string;
  rewardPenaltyType: string;
  counts: string;
  userName: string;
  language: string;
}

export interface EmployeeRewardAndPenaltyReportDto {
  factory: string;
  department: string;
  department_Name: string;
  employee_ID: string;
  localFullName: string;
  date: string;
  rewardPenaltyType: string;
  reasonCode: string;
  year_Month: string;
  countsOf: number;
  remark: string;
}

export interface EmployeeRewardAndPenaltyReportSource {
  param: EmployeeRewardAndPenaltyReportParam;
  totalRows: number;
  start_Year_Month: Date;
  end_Year_Month: Date;
  start_Date: Date;
  end_Date: Date;
}
