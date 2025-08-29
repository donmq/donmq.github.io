import { Pagination } from '@utilities/pagination-utility';

export interface OvertimeApplicationMaintenance_Param {
  useR_GUID: string;
  factory: string;
  department: string;
  employee_Id: string;
  work_Shift_Type: string;
  overtime_Date: string;
  overtime_Date_Str: string;
  overtime_Date_From: string;
  overtime_Date_From_Str: string;
  overtime_Date_To: string;
  overtime_Date_To_Str: string;
  lang: string;
}
export interface OvertimeApplicationMaintenance_Main {
  factory: string;
  department_Code: string;
  department_Name: string;
  department_Code_Name: string;
  useR_GUID: string;
  employee_Id: string;
  local_Full_Name: string;
  work_Shift_Type: string;
  work_Shift_Type_Name: string;
  overtime_Date: Date;
  overtime_Date_Str: string;
  clock_In: Date;
  clock_In_Str: string;
  clock_Out: Date;
  clock_Out_Str: string;
  overtime_Start: Date;
  overtime_Start_Str: string;
  overtime_End: Date;
  overtime_End_Str: string;
  overtime_Hours: string;
  night_Hours: string;
  training_Hours: string;
  night_Eat_Times: number;
  update_By: string;
  update_Time: Date;
  update_Time_Str: string;
  isDuplicated: boolean;
}
export interface OvertimeApplicationMaintenance_MainMemory {
  param: OvertimeApplicationMaintenance_Param
  pagination: Pagination
  data: OvertimeApplicationMaintenance_Main[]
}
export interface OvertimeApplicationMaintenance_TypeheadKeyValue {
  key: string;
  useR_GUID: string;
  name: string;
  department: string;
  department_Name: string;
}
