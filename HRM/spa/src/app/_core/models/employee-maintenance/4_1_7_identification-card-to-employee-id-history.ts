import { Pagination } from "@utilities/pagination-utility";

export interface HRMS_Emp_IDcard_EmpID_HistoryDto {
  nationality: string;
  identification_Number: string;
  local_Full_Name: string;
  division: string;
  factory: string;
  employee_ID: string;
  department: string;
  assigned_Division: string;
  assigned_Factory: string;
  assigned_Employee_ID: string;
  assigned_Department: string;
  onboard_Date: string;
  resign_Date: string;
  update_By: string;
  update_Time: string;
}

export interface IdentificationCardToEmployeeIDHistoryParam {
  division: string;
  factory: string;
  employee_ID: string;
  nationality: string;
  identification_Number: string;
  lang: string;
}

export interface IdentificationCardToEmployeeIDHistorySource {
  param: IdentificationCardToEmployeeIDHistoryParam
  pagination: Pagination
  data: HRMS_Emp_IDcard_EmpID_HistoryDto[]
}
