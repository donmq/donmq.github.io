import { Pagination } from "@utilities/pagination-utility";

export interface EmployeeLunchBreakTimeSettingParam {
  factory: string;
  in_Service: string;
  employee_ID: string;
  department_From: string;
  department_To: string;
  lang: string;
}

export interface HRMS_Att_LunchtimeDto {
  useR_GUID: string;
  factory: string;
  department_Code: string;
  department_Name: string;
  department_Code_Name: string;
  employee_ID: string;
  local_Full_Name: string;
  lunch_Start: string;
  lunch_End: string;
  update_By: string;
  update_Time: string;
}

export interface EmployeeLunchBreakTimeSettingUpload {
  file: File;
}

export interface EmployeeLunchBreakTimeSettingSource {
  param: EmployeeLunchBreakTimeSettingParam;
  selectedKey: string;
  pagination: Pagination;
  data: HRMS_Att_LunchtimeDto[];
}
