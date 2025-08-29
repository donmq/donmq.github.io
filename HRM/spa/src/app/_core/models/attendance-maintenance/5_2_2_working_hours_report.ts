
export interface WorkingHoursReportParam {
  factory: string;
  department: string;
  date_From: string;
  date_To: string;
  salary_WorkDays: number;
  lang: string;
}

export interface WorkingHoursReportDto {
  department: string;
  department_Name: string;
  employee_ID: string;
  local_Full_Name: string;
  actual_Work_Days: number;
  night_Shift_Allowance_Times: number;
  lang: string;
}

export interface WorkingHoursReportSource {
  dateFrom: Date;
  dateTo: Date;
  param: WorkingHoursReportParam,
  source?: WorkingHoursReportDto,
  total: number;
}
