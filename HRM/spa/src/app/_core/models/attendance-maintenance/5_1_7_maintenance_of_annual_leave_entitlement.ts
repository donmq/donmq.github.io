import { Pagination } from "@utilities/pagination-utility";

export interface MaintenanceOfAnnualLeaveEntitlement {
  useR_GUID: string;
  annual_Start: string;
  annual_Start_Date: Date;
  annual_End: string;
  annual_End_Date: Date;
  factory: string;
  department_Code: string;
  department_Name: string;
  department_Code_Name: string;
  employee_ID: string;
  local_Full_Name: string;
  leave_Code: string;
  leave_Code_Old: string;
  leave_Code_Name: string;
  previous_Hours: string;
  year_Hours: string;
  total_Hours: number;
  total_Days: number;
  update_By: string;
  update_Time: string | Date;
  isDuplicate: boolean;
  id: string;
  isDisabled: boolean;
}

export interface EmpLeaveInfo {
  useR_GUID: string;
  local_Full_Name: string;
  department: string;
}

export interface MaintenanceOfAnnualLeaveEntitlementParam {
  factory: string;
  department: string;
  employee_ID: string;
  leave_Code: string;
  availableRange_Start: string;
  availableRange_End: string;
  language: string;
}

export interface AnnualLeaveDetailParam {
  factory: string;
  availableRange_Start: string;
  employee_ID: string;
  leave_Code: string;
  language: string;
}

export interface MaintenanceOfAnnualLeaveEntitlementMemory {
  params: MaintenanceOfAnnualLeaveEntitlementParam;
  pagination: Pagination;
  datas: MaintenanceOfAnnualLeaveEntitlement[];
}
