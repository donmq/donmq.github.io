import { Pagination } from "@utilities/pagination-utility";

export interface HRMS_Emp_Unpaid_LeaveDto {
  division: string;
  division_Str: string;
  factory: string;
  employee_ID: string;
  department: string;
  local_Full_Name: string;
  onboard_Date: string;
  ordinal_Number: number;
  leave_Reason: string;
  leave_Reason_Str: string;
  leave_Start: string;
  leave_End: string;
  continuation_of_Insurance: boolean;
  seniority_Retention: boolean;
  annual_Leave_Seniority_Retention: boolean;
  effective_Status: boolean;
  remark: string;
  update_By: string;
  update_Time: string;
}

export interface HRMS_Emp_Unpaid_LeaveModel extends HRMS_Emp_Unpaid_LeaveDto {
  leaveStartDate: string;
  leaveEndDate: string;

}

export interface UnpaidLeaveParam {
  division: string;
  factory: string;
  department: string;
  employee_ID: string;
  local_Full_Name: string;
  onboard_Date: string;
  leave_Reason: string;
  leave_Start: string;
  leave_Start_From: string;
  leave_Start_To: string;
  leave_End: string;
  leave_End_From: string;
  leave_End_To: string;
  lang: string;
}

export interface AddAndEditParam {
  division: string;
  factory: string;
  department: string;
  employee_ID: string;
  local_Full_Name: string;
  onboard_Date: string;
  leave_Reason: string;
  leave_Start: string;
  leave_End: string;
  ordinal_Number: number;
  continuation_of_Insurance: boolean;
  seniority_Retention: boolean;
  annual_Leave_Seniority_Retention: boolean;
  effective_Status: boolean;
  remark: string;
  update_By: string;
  update_Time: string;
}

export interface UnpaidLeaveSource {
  param: UnpaidLeaveParam
  onboardDate: Date;
  leaveStartFrom: Date;
  leaveStartTo: Date;
  leaveEndFrom: Date;
  leaveEndTo: Date;
  pagination: Pagination
  data: HRMS_Emp_Unpaid_LeaveDto[]
}
