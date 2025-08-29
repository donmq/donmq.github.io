import { Pagination } from "@utilities/pagination-utility";

export interface HRMS_Att_Change_RecordDto {
  useR_GUID: string;
  factory: string; // (*)
  department_Code: string;
  department_Name: string;
  department_Code_Name: string;
  employee_ID: string; // (*)
  local_Full_Name: string;
  att_Date: Date; // Date (*)
  att_Date_Str: string; // Date (*)
  work_Shift_Type: string; // (*)
  work_Shift_Type_Str: string; // display main
  leave_Code: string; // Attendance (*)
  leave_Code_Str: string; // display main
  before_Leave_Code: string; // temp for edit
  after_Leave_Code: string; // temp for edit
  reason_Code: string; // Update Reason (*)
  reason_Code_Str: string; // display main
  clock_In: string; // from HRMS_Att_Change_Reason
  modified_Clock_In: string; // Clock_In (*)
  clock_Out: string; //from HRMS_Att_Change_Reason
  modified_Clock_Out: string; // Clock_Out (*)
  overtime_ClockIn: string; // HRMS_Att_Change_Reason.Overtime_ClockIn
  modified_Overtime_ClockIn: string; // HRMS_Att_Change_Record.Overtime_ClockIn (*)
  overtime_ClockOut: string; // HRMS_Att_Change_Reason.Overtime_ClockOut
  modified_Overtime_ClockOut: string; // HRMS_Att_Change_Record.Overtime_ClockOut (*)
  days: number;
  before_Days: number; // temp for edit
  after_Days: number; // temp for edit
  holiday: string;
  holiday_Str: string;
  update_By: string;
  update_Time: Date;
  update_Time_Str: string;
  id: string;
  isAttDate: boolean;
  modified_Clock_In_Old :string;
  modified_Clock_Out_Old :string;
  modified_Overtime_ClockIn_Old :string;
  modified_Overtime_ClockOut_Old :string;
}
export interface HRMS_Att_Change_Record_Params {
  lang: string;
  factory: string;
  employee_ID: string;
  department: string;
  work_Shift_Type: string;
  leave_Code: string; // Attendance
  reason_Code: string; // Update_Reason
  date_Start: string;
  date_Start_Str: string;
  date_End: string;
  date_End_Str: string;
}
export interface HRMS_Att_Change_Record_Delete_Params {
  useR_GUID: string;
  lang: string;
  factory: string;
  att_Date: string;
  employee_ID: string;
  days: number;
}

export interface AttendanceChangeRecordMaintenanceSource {
  param: HRMS_Att_Change_Record_Params
  pagination: Pagination
  data: HRMS_Att_Change_RecordDto[]
}


export interface EmployeeInfo {
  useR_GUID: string;
  local_Full_Name: string;
  department_Code: string;
  department_Code_Name: string;
}
