import { Pagination } from "@utilities/pagination-utility";

export interface AttendanceAbnormalityDataMaintenanceParam {
  factory: string;
  department: string;
  employee_ID: string;
  work_Shift_Type: string;
  list_Attendance: string[];
  reason_Code: string;
  att_Date_From: string;
  att_Date_From_Str: string;
  att_Date_To: string;
  att_Date_To_Str: string;
  lang: string;
}

export interface HRMS_Att_Temp_RecordDto {
  useR_GUID: string;
  factory: string;
  department_Code: string;
  department_Name: string;
  department_Code_Name: string;
  employee_ID: string;
  local_Full_Name: string;
  att_Date: Date;
  att_Date_Str: string;
  work_Shift_Type: string;
  work_Shift_Type_Str: string;
  leave_Code: string;
  leave_Code_Str: string;
  reason_Code: string;
  reason_Code_Str: string;
  clock_In: string;
  clock_Out: string;
  overtime_ClockIn: string;
  overtime_ClockOut: string;
  modified_Overtime_ClockOut: string;
  modified_Overtime_ClockIn: string;
  modified_Clock_In: string;
  modified_Clock_Out: string;
  modified_Overtime_ClockOut_Old: string;
  modified_Overtime_ClockIn_Old: string;
  modified_Clock_In_Old: string;
  modified_Clock_Out_Old: string;
  days: string;
  holiday: string;
  holiday_Str: string;
  update_By: string;
  update_Time: Date;
  update_Time_Str: string;
}

export interface EmployeeData {
  useR_GUID: string;
  local_Full_Name: string;
  department_Code: string;
  department_Code_Name: string;
}

export interface AttendanceAbnormalityDataMaintenanceSource {
  pagination: Pagination
  param: AttendanceAbnormalityDataMaintenanceParam,
  data: HRMS_Att_Temp_RecordDto[]
}
