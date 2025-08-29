import { LeaveAllowance } from './5_1_22_monthly-attendance-data-maintenance-active-employees';
import { Pagination } from "@utilities/pagination-utility";

export interface MonthlyAttendanceSettingParam_Main {
  factory: string;
  effective_Month: string;
  effective_Month_Str: string;
}
export interface MonthlyAttendanceSettingParam_Form {
  factory: string;
  effective_Month: Date;
  effective_Month_Str: string;
}
export interface MonthlyAttendanceSettingParam_SubData {
  leaveData: HRMS_Att_Use_Monthly_LeaveDto[];
  allowanceData: HRMS_Att_Use_Monthly_LeaveDto[];
}
export interface HRMS_Att_Use_Monthly_LeaveDto {
  factory: string;
  effective_Month: Date;
  effective_Month_Str: string;
  leave_Type: string;
  code: string;
  seq: number;
  month_Total: boolean;
  year_Total: boolean | null;
  update_By: string;
  update_Time?: string;
  method: string;
  max_Effective_Month: Date | string;
  is_Function_Edit: boolean;
}

export interface ParamSource {
  paramMain: ParamMain;
}

export interface ParamMain {
  paramSearch: MonthlyAttendanceSettingParam_Main;
  data: HRMS_Att_Use_Monthly_LeaveDto[];
  pagination: Pagination;
}
export interface ParamForm {
  param : MonthlyAttendanceSettingParam_Form
  dataLeaveTemps: HRMS_Att_Use_Monthly_LeaveDto[];
  dataAllowanceTemps: HRMS_Att_Use_Monthly_LeaveDto[];
}
