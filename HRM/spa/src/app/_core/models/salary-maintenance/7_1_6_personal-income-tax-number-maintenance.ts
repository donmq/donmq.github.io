import { Pagination } from '@utilities/pagination-utility';

export interface PersonalIncomeTaxNumberMaintenanceParam {
  factory: string;
  year_Date: string;
  year: string;
  employee_ID: string;
  lang: string;
}

export interface PersonalIncomeTaxNumberMaintenanceDto {
  useR_GUID: string;
  factory: string;
  year_Date: Date;
  year: string;
  employee_ID: string;
  department_Code: string;
  department_Name: string;
  department_Code_Name: string;
  local_Full_Name: string;
  taxNo: string;
  dependents: string;
  update_By: string;
  update_Time: string;
}

export interface PersonalIncomeTaxNumberMaintenanceSource {
  pagination: Pagination
  param: PersonalIncomeTaxNumberMaintenanceParam,
  selectedData: PersonalIncomeTaxNumberMaintenanceDto,
  data: PersonalIncomeTaxNumberMaintenanceDto[]
}
