import { Pagination } from '@utilities/pagination-utility';
export interface HRMS_Emp_BlacklistDto {
  useR_GUID: string;
  maintenance_Date: Date | string;
  nationality: string;
  identification_Number: string;
  local_Full_Name: string;
  resign_Reason: string;
  description: string;
  update_By: string;
  update_Time: Date;
}
export interface HRMS_Emp_BlacklistParam {
  nationality: string;
  identification_Number: string;
}

export interface HRMS_Emp_Blacklist_MainMemory {
  param: HRMS_Emp_BlacklistParam;
  pagination: Pagination
  data: HRMS_Emp_BlacklistDto[];
}

export interface HRMS_Emp_BlacklistModel {
  maintenance_Date: Date | string;
  nationality: string;
  identification_Number: string;
  local_Full_Name: string;
  resign_Reason: string;
  description: string;
  update_By: string;
  update_Time: Date;
}
