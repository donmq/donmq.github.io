export interface HRDailyReportParam {
  factory: string;
  date: string;
  level: string;
  level_Name: string;
  permissionGroups: string[];
  lang: string;
}
export interface HRDailyReportResult {
  HRDailyReport: HRDailyReport[];
  headCount: number;
}
export interface HRDailyReport {
  department: string;
  department_Name: string;
  expectedAttendance: number;
  expectedAttendanceExcluding5DaysAbsenteeism: number;
  expectedAttendanceExcluding5DaysAbsenteeismAndMaternityLeave: number;
  staff: number;
  managers: number;
  technicians: number;
  waterSpiders: number;
  assistants: number;
  personalLeave: number; // Personal_Cnt
  unpaidLeave: number;
  sickLeave: number;
  absenteeism: number;
  workStoppage: number;
  annualLeaveCompany: number;
  annualLeaveEmployee: number;
  otherLeave: number;
  maternityLeave: number;
  prenatalCheckupLeave: number;
  compensatoryMaternityLeave: number;
  businessTrip: number;
  actualAttendance: number;
  newRecruit: number;
  resigned: number;
  expectedAttendanceTomorrow: number;
  expectedAttendanceTomorrowExcluding5days: number;
  _8HourWorkForPregnantEmployees: number;
  _7HourWorkForPregnantEmployees: number;
  _7HourWorkForEmployeesWithBabiesUnder12Months: number;
  _8HourWorkForEmployeesWithBabiesUnder12Months: number;
  employee_Absences_5Days: number;
  totalPersonalSickAndAbsenteeismLeave: number;
  averageLeaveCountPersonalSickAndAbsenteeism: number;
  headCount: number;
}
export interface HRDailyReportCount {
  queryResult: number;
  headCount: number;
  monthlyAbsenteeism: number;
}
export interface HRDailyReportDept_values_Emp_Cnt {
  dept_values: string;
  emp_Cnt: number;
}
export interface HRDailyReportStaffCount {
  department: string;
  count: number;
}
export interface HRDailyReportStaffSum {
  department: string;
  sum: number;
}
export interface HRDailyReportD {
  factory: string;
  department: string;
}

export interface HRDailyReportSource {
  param: HRDailyReportParam;
  resultCount: HRDailyReportCount;
}

export interface HRDailyReportCount {
  queryResult: number;
  headCount: number;
  monthlyAbsenteeism: number;
}
