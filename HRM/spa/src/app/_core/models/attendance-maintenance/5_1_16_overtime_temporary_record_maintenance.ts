import { Pagination } from "@utilities/pagination-utility";

export interface HRMS_Att_Overtime_TempDto {
  useR_GUID: string;
  factory: string;
  department_Code: string;
  department_Name: string;
  department_Code_Name: string;
  employee_ID: string;
  local_Full_Name: string;
  date: Date;
  date_Str: string;
  work_Shift_Type: string;
  work_Shift_Type_Str: string;
  shift_Time: string;
  clock_In_Time: string;
  clock_Out_Time: string;
  overtime_Start: string;
  overtime_End: string;
  overtime_Hours: number;
  night_Hours: number;
  night_Overtime_Hours: number;
  training_Hours: number;
  night_Eat: number;
  holiday: string;
  holiday_Str: string;
  update_By: string;
  update_Time: Date;
  update_Time_Str: string;
}

export interface HRMS_Att_Overtime_TempParam {
  factory: string;
  department: string;
  employee_ID: string;
  shift: string;
  dateFrom: string;
  dateTo: string;
  dateFrom_Date: string;
  dateTo_Date: string;
  lang: string;
}

export interface OvertimeTemporarySource {
  isEdit: boolean;
  source?: HRMS_Att_Overtime_TempDto;
  paramQuery: HRMS_Att_Overtime_TempParam;
  dataMain: HRMS_Att_Overtime_TempDto[];
  pagination: Pagination;
}

export interface OvertimeTempPersonal {
  useR_GUID: string;
  local_Full_Name: string;
  department_Code: string;
  department_Code_Name: string;
}
export interface ClockInOutTempRecord {
  clock_In_Time: string;
  clock_Out_Time: string;
}
