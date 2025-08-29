import { Pagination } from "@utilities/pagination-utility";

export interface LoanedMonthlyAttendanceDataMaintenanceParam {
  factory: string;
  att_Month_From: string;
  att_Month_To: string;
  department: string;
  employee_ID: string;
  salary_Days: string;
  lang: string;
}

export interface LoanedMonthlyAttendanceDataMaintenanceDto {
  useR_GUID: string;
  division: string;
  pass: boolean;
  pass_Str: string;
  factory: string;
  att_Month: string;
  department: string;
  department_Code: string;
  department_Name: string;
  employee_ID: string;
  local_Full_Name: string;
  resign_Status: string;
  delay_Early: number;
  no_Swip_Card: number;
  food_Expenses: number;
  night_Eat_Times: number;
  permission_Group: string;
  salary_Type: string;
  salary_Days: number;
  actual_Days: number;
  leaves: DetailDisplay[];
  allowances: DetailDisplay[];
  update_By: string;
  update_Time: string;
}

export interface LoanedMonthlyAttendanceDataMaintenanceSource {
  pagination: Pagination
  att_Month_From: Date;
  att_Month_To: Date;
  isEdit: boolean;
  isQuery: boolean;
  param: LoanedMonthlyAttendanceDataMaintenanceParam,
  source?: LoanedMonthlyAttendanceDataMaintenanceDto,
  data: LoanedMonthlyAttendanceDataMaintenanceDto[]
}

export interface DetailDisplay {
  code: string;
  codeName: string;
  days: number;
  total: number;
}
