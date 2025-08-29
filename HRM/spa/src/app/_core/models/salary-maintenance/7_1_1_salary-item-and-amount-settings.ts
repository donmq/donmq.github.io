import { Pagination } from '@utilities/pagination-utility';

export interface SalaryItemAndAmountSettings_MainParam {
  factory: string;
  salary_Type: string;
  effective_Month: string;
  effective_Month_Str: string;
  permission_Group: string[];
  formType: string
  lang: string;
}
export interface SalaryItemAndAmountSettings_MainData {
  factory: string;
  effective_Month: string;
  permission_Group: string;
  permission_Group_Name: string;
  salary_Type: string;
  salary_Type_Name: string;
  salary_Days: string;
  seq: string;
  salary_Item: string;
  salary_Item_Name: string;
  kind: string;
  kind_Name: string;
  insurance: string
  amount: string;
  update_By: string;
  update_Time: string;
  is_Editable: boolean;
}
export interface SalaryItemAndAmountSettings_SubParam {
  factory: string;
  effective_Month: string;
  effective_Month_Str: string;
  permission_Group: string;
  salary_Type: string;
  salary_Days: string;
}
export interface SalaryItemAndAmountSettings_SubData {
  seq: string;
  salary_Item: string;
  kind: string;
  insurance: string
  amount: string | number;
  update_By: string;
  update_Time: Date;
  update_Time_Str: string;
  is_Duplicate: boolean
}
export interface SalaryItemAndAmountSettings_Update {
  param : SalaryItemAndAmountSettings_SubParam
  data : SalaryItemAndAmountSettings_SubData[]
}
export interface SalaryItemAndAmountSettings_Memory {
  param: SalaryItemAndAmountSettings_MainParam
  pagination: Pagination
  selectedData: SalaryItemAndAmountSettings_Update
  data: SalaryItemAndAmountSettings_MainData[]
}
