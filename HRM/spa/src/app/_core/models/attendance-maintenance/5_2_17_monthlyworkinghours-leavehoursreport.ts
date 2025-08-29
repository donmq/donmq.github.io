export interface MonthlyWorkingHoursLeaveHoursReportDto {
  factory: string;
  yearMonth: string;
  permissionGroup: string;
  gs14: number;
  gs17: number;
  gs26: number;
  gs27: number;
  ar1: number;
  ar2: number;
  ar3: number;
  ar4: number;
  arAb: number;
}
export interface MonthlyWorkingHoursLeaveHoursReportParam {
  factory: string;
  yearMonth: string;
  permissionGroup: string[];
  option: string;
  language: string;
  userName: string;
}

export interface MonthlyWorkingHoursLeaveHoursReportSource {
  param: MonthlyWorkingHoursLeaveHoursReportParam;
  yearMonth_Value: Date;
  totalRows: number;
}
