export interface AbsenceDailyReportParam {
  factory: string;
  date: string;
  lang: string;
}

export interface AbsenceDailyReport {
  department: string;
  department_Name: string;
  employee_ID: string;
  local_Full_Name: string;
  leave: string;
  days: number | null;
  monthlyLeaveTotal: number;
  monthlyAbsentTotal: number;
}

export interface AbsenceDailyReportTodaysNewRecruits {
  department: string;
  department_Name: string;
  employee_ID: string;
  local_Full_Name: string;
}

export interface AbsenceDailyReportTodaysResigning extends AbsenceDailyReportTodaysNewRecruits {
}

export interface AbsenceDailyReportSource {
  param: AbsenceDailyReportParam;
  resultCount: AbsenceDailyReportCount;
}

export interface AbsenceDailyReportCount {
  queryResult: number;
  recruits: number;
  resigning: number;
}
