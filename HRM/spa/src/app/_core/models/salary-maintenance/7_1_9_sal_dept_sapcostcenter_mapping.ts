import { Pagination } from "@utilities/pagination-utility";

export interface Sal_Dept_SAPCostCenter_MappingDTO {
  cost_Year: string;
  cost_Year_Str: string;
  factory: string;
  cost_Code: string;
  code_Name: string;
  department: string;
  department_Old: string;
  department_New: string;
  department_Name: string;
  update_By: string;
  update_Time: string;
}
export interface Sal_Dept_SAPCostCenter_MappingParam {
  factory: string;
  year: string;
  year_Str: string;
  department: string;
  costCenter: string;
  language: string;
}
export interface Sal_Dept_SAPCostCenter_MappingSource {
  selectedData: Sal_Dept_SAPCostCenter_MappingDTO;
  param: Sal_Dept_SAPCostCenter_MappingParam;
  data: Sal_Dept_SAPCostCenter_MappingDTO[];
  pagination: Pagination;
}
