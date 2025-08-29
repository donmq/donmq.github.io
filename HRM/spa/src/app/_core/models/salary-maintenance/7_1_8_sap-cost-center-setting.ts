import { Pagination } from "@utilities/pagination-utility";

export interface SAPCostCenterSettingDto {
  factory: string;
  company_Code: string;
  year_Date: Date; // cost_year
  year: string; // cost_year
  group: string;
  cost_Code: string;
  kind: string; // Function
  kind_Name: string;
  profit_Center: string; //  Profit_Center code
  code_Name: string; // CostCenterChineseDescription
  code_Name_EN: string; // CostCenterEnglishDescription
  update_By: string;
  update_Time: string;
}

export interface SAPCostCenterSettingParam {
  factory: string;
  company_Code: string;
  costYear: string;
  language: string;
}

export interface CheckDuplicateParam {
    factory: string;
    cost_Year: string;
    company_Code: string;
    cost_Code: string;
}

export interface DeleteParam extends CheckDuplicateParam {
}

export interface ExistedDataParam extends CheckDuplicateParam {
}

export interface SAPCostCenterSettingSource {
  param: SAPCostCenterSettingParam
  pagination: Pagination
  data: SAPCostCenterSettingDto[]
  selectedData: SAPCostCenterSettingDto
}
