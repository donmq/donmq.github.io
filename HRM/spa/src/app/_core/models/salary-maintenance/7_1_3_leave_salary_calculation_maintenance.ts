import { Pagination } from "./../../utilities/pagination-utility";
export interface LeaveSalaryCalculationMaintenanceDTO {
  factory: string;
  leave_Code: string;
  leave_Code_Name: string;
  salary_Rate: number;
  update_By: string;
  update_Time: string;
}

export interface LeaveSalaryCalculationMaintenanceParam {
  factory: string;
  language: string
}

export interface LeaveSalaryCalculationMaintenance_Source {
  param: LeaveSalaryCalculationMaintenanceParam,
  isEdit: boolean;
  data: LeaveSalaryCalculationMaintenanceDTO
}

export interface LeaveSalaryCalculationMaintenance_Basic {
  param: LeaveSalaryCalculationMaintenanceParam
  pagination: Pagination
  selectedData: LeaveSalaryCalculationMaintenanceDTO
  data: LeaveSalaryCalculationMaintenanceDTO[]
}
