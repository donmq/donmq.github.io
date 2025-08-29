import { Pagination } from "./../../utilities/pagination-utility";
export interface BankAccountMaintenanceDto {
  useR_GUID: string;
  factory: string;
  employee_ID: string;
  local_Full_Name: string;
  bank_Code: string;
  bankNo: string;
  create_Date: string;
  update_By: string;
  update_Time: string;
}

export interface BankAccountMaintenanceParam {
  factory: string;
  employee_ID: string;
  language: string;
}

export interface BankAccountMaintenance_Source {
  param: BankAccountMaintenanceParam,
  isEdit: boolean;
  data: BankAccountMaintenanceDto
}
export interface BankAccountMaintenance_Basic {
  param: BankAccountMaintenanceParam
  pagination: Pagination
  data: BankAccountMaintenanceDto[]
  selectedData: BankAccountMaintenanceDto
}
export interface BankAccountMaintenanceUpload {
  file: File;
  language: string;
}
