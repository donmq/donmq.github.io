import { Pagination } from "@utilities/pagination-utility";

export interface EmployeeTransferHistoryParam {
  division_After: string;
  factory_After: string;
  employee_ID_After: string;
  department_After: string;
  effective_Date_Start: string;
  effective_Date_End: string;
  assigned_Division_After: string;
  assigned_Factory_After: string;
  assigned_Department_After: string;
  assigned_Employee_ID_After: string;
  local_Full_Name: string;
  effective_Status: number;
  lang: string;
}

export interface EmployeeTransferHistoryDetail extends EmployeeTransferHistoryDTO {
  checked: boolean;
  isEffectiveConfirm : boolean
}

export interface EmployeeTransferHistoryDetele {
  history_GUID: string;
  effective_Status: boolean;
}

export interface EmployeeTransferHistoryEffectiveConfirm {
  useR_GUID: string;
  effective_Status: boolean;
  effective_Date: string;
  history_GUID: string;
}

export interface EmployeeTransferHistorySource {
  currentPage: number,
  param: EmployeeTransferHistoryParam,
  basicCode: EmployeeTransferHistoryDetail
}

export interface EmployeeTransferHistory {
  param: EmployeeTransferHistoryParam
  pagination: Pagination
  data: EmployeeTransferHistoryDetail[]
}

export interface EmployeeTransferHistoryDTO {
  useR_GUID: string;
  history_GUID: string;
  data_Source: string;
  data_Source_Name: string;
  nationality_Before: string;
  nationality_After: string;
  identification_Number_Before: string;
  identification_Number_After: string;
  local_Full_Name_Before: string;
  local_Full_Name_After: string;
  division_Before: string;
  division_After: string;
  factory_Before: string;
  factory_After: string;
  employee_ID_Before: string;
  employee_ID_After: string;
  department_Before: string;
  department_After: string;
  assigned_Division_Before: string;
  assigned_Division_After: string;
  assigned_Factory_Before: string;
  assigned_Factory_After: string;
  assigned_Employee_ID_Before: string;
  assigned_Employee_ID_After: string;
  assigned_Department_Before: string;
  assigned_Department_After: string;
  position_Grade_Before: number;
  position_Grade_After: number;
  position_Title_Before: string;
  position_Title_After: string;
  work_Type_Before: string;
  work_Type_After: string;

  actingPosition_Start_Before: Date;
  actingPosition_Start_Before_Str: string;

  actingPosition_End_Before: Date;
  actingPosition_End_Before_Str: string;

  actingPosition_Start_After: Date;
  actingPosition_Start_After_Str: string;

  actingPosition_End_After: Date;
  actingPosition_End_After_Str: string;

  reason_for_Change: string;
  seq: number;
  effective_Date: Date;
  effective_Date_Str: string;
  effective_Status: boolean;
  update_By: string;
  update_Time: Date;
}
