import { Pagination } from '@utilities/pagination-utility';

export interface FinSalaryAttributionCategoryMaintenance_Param {
  factory: string;
  department: string;
  kind: string;
  kind_Code: string;
  kind_Code_List: string[];
  salary_Category: string
  function_Type: string;
  lang: string;
}
export interface FinSalaryAttributionCategoryMaintenance_Data {
  factory: string;
  department: string;
  department_Name: string;
  department_Code_Name: string;
  kind: string;
  kind_Name: string;
  kind_Code: string;
  kind_Code_Name: string;
  salary_Category: string
  salary_Category_Name: string
  update_By: string;
  update_Time: string;
  is_Duplicate: boolean
}
export interface FinSalaryAttributionCategoryMaintenance_Update {
  param: FinSalaryAttributionCategoryMaintenance_Param
  data: FinSalaryAttributionCategoryMaintenance_Data[]
}
export interface FinSalaryAttributionCategoryMaintenance_Memory {
  param: FinSalaryAttributionCategoryMaintenance_Param
  pagination: Pagination
  data: FinSalaryAttributionCategoryMaintenance_Data[]
}
