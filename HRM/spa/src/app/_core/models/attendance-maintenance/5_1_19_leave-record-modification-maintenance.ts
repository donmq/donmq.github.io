import { Pagination } from "@utilities/pagination-utility";

export interface Leave_Record_Modification_MaintenanceDto {
  useR_GUID: string;
  factory: string;
  department_Code: string;
  department_Name: string;
  department_Code_Name: string;
  employee_ID: string;
  local_Full_Name: string;
  work_Shift_Type: string;
  work_Shift_Type_Str: string;
  leave_Code: string;
  leave_Code_Str: string;
  leave_Date: Date;
  leave_Date_Str: string;
  days: number;
  update_By: string;
  update_Time: Date;
  update_Time_Str: string;
  isEdit: boolean;
  isLeaveDate: boolean;
  id: string;
}
export interface HRMS_Att_Leave_MaintainSearchParamDto {
  lang: string;
  factory: string;
  department: string;
  employee_ID: string;
  permission_Group: string;
  leave: string; // HRMS_Att_Leave_Maintain.Leave_End
  date_Start: string;
  date_Start_Str: string;
  date_End: string;
  date_End_Str: string;
  work_Shift_Type: string;
  leave_Date_Str: string;
}

export interface HRMS_Att_Leave_MaintainDeleteParamDto {
  lang: string;
  useR_GUID: string;
  factory: string;
  employee_ID: string;
  leave_code: string;
  leave_Date_Str: string;
  work_Shift_Type: string;
}

export interface LeaveRecordModificationMaintenanceSource {
  param: HRMS_Att_Leave_MaintainSearchParamDto
  pagination: Pagination
  selectedData: Leave_Record_Modification_MaintenanceDto
  data: Leave_Record_Modification_MaintenanceDto[]
}

