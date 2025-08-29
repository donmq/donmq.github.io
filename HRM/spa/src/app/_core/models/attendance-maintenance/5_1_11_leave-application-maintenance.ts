import { Pagination } from '@utilities/pagination-utility';

export interface LeaveApplicationMaintenance_Param {
  useR_GUID: string;
  factory: string;
  department: string;
  employee_Id: string;
  leave: string;
  leave_Date_From: string;
  leave_Date_From_Str: string;
  leave_Date_To: string;
  leave_Date_To_Str: string;
  lang: string;
}
export interface LeaveApplicationMaintenance_Main {
  factory: string;
  department_Code: string;
  department_Name: string;
  department_Code_Name: string;
  useR_GUID: string;
  employee_Id: string;
  local_Full_Name: string;
  leave_Code: string;
  leave_Name: string;
  leave_Start_Date: Date;
  leave_Start_Time: Date;
  leave_Start_Datetime: Date;
  leave_Start: string;
  min_Start: string;
  leave_End_Date: Date;
  leave_End_Time: Date;
  leave_End_Datetime: Date;
  leave_End: string;
  min_End: string;
  days: string;
  update_Time: Date;
  update_Time_Str: string;
  isDuplicated: boolean;
  isErrorTime: boolean;
}
export interface LeaveApplicationMaintenance_MainMemory {
  param: LeaveApplicationMaintenance_Param
  pagination: Pagination
  data: LeaveApplicationMaintenance_Main[]
}
export interface LeaveApplicationMaintenance_TypeheadKeyValue {
  key: string;
  useR_GUID: string;
  name: string;
  department: string;
  department_Name: string;
}
