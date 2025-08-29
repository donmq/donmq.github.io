import { Pagination } from "@utilities/pagination-utility";

export interface HRMS_Emp_ResignationDto {
  user_GUID: string;
  division: string;
  division_Str: string;
  factory: string;
  factory_Str: string;
  employee_ID: string;
  nationality: string;
  local_Full_Name: string;
  identification_Number: string;
  onboard_Date: string;
  resign_Date: string;
  resignation_Type: string;
  resignation_Type_Str: string;
  resign_Reason: string;
  resign_Reason_Str: string;
  remark: string;
  blacklist: boolean | null;
  blacklist_Str: string;
  verifier: string;
  verifier_Name: string;
  verifier_Title: string;
  update_By: string;
  update_Time: string;
}

export interface HRMS_Emp_ResignationFormDto {
  nationality: string;
  local_Full_Name: string;
  identification_Number: string;
  onboard_Date: string;
}

export interface ResignationManagementParam {
  division: string;
  factory: string;
  employee_ID: string;
  local_Full_Name: string;
  startDate: string;
  endDate: string;
  lang: string;
}

export interface ResignAddAndEditParam {
  user_GUID: string;
  division: string;
  factory: string;
  employee_ID: string;
  nationality: string;
  identification_Number: string;
  local_Full_Name: string;
  resign_Date: string;
  onboard_Date: string;
  resignation_Type: string;
  resign_Reason: string;
  remark: string;
  blacklist: boolean | null;
  verifier: string;
  verifier_Name: string;
  verifier_Title: string;
  update_By: string;
  update_Time: string;
}

export interface ResignationManagementSource {
  pagination: Pagination
  startDate: Date;
  endDate: Date;
  param: ResignationManagementParam,
  source?: HRMS_Emp_ResignationDto,
  data: HRMS_Emp_ResignationDto[]
}
