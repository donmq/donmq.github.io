import { Pagination } from "@utilities/pagination-utility";

export interface SalaryAdditionsAndDeductionsInputDto {
  useR_GUID: string;
  factory: string;
  division: string;
  sal_Month: Date;
  sal_Month_Str: string;
  department_Code: string;
  department_Name: string;
  department_Code_Name: string;
  employee_ID: string;
  local_Full_Name: string;
  addDed_Type: string;
  addDed_Type_Str: string;
  addDed_Item: string;
  addDed_Item_Str: string;
  currency: string;
  currency_Str: string;
  amount: number;
  update_By: string;
  update_Time: Date;
  update_Time_Str: string;
}

export interface SalaryAdditionsAndDeductionsInput_Param {
  factory: string;
  sal_Month: string;
  addDed_Type: string;
  addDed_Item: string;
  employee_ID: string;
  department: string;
  language: string;
}

export interface SalaryAdditionsAndDeductionsInput_Upload {
  file: File;
  language: string;
}

export interface SalaryAdditionsAndDeductionsInput_Source {
  param: SalaryAdditionsAndDeductionsInput_Param,
  isEdit: boolean;
  data: SalaryAdditionsAndDeductionsInputDto
}

export interface SalaryAdditionsAndDeductionsInput_Basic {
  param: SalaryAdditionsAndDeductionsInput_Param
  pagination: Pagination
  data: SalaryAdditionsAndDeductionsInputDto[]
  selectedData: SalaryAdditionsAndDeductionsInputDto
}

export interface SalaryAdditionsAndDeductionsInput_Personal {
  useR_GUID: string;
  local_Full_Name: string;
  department: string;
}
