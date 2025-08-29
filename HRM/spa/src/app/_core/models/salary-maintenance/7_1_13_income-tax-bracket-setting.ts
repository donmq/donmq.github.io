import { Pagination } from "@utilities/pagination-utility";

export interface IncomeTaxBracketSettingParam {
  nationality: string;
  tax_Code: string;
  start_Effective_Month: string;
  end_Effective_Month: string;
  language: string;
}
export interface IncomeTaxBracketSettingMain {
  nation: string;
  effective_Month: Date;
  effective_Month_Str: string;
  tax_Code: string;
  tax_Code_Name: string;
  type: string;
  tax_Level: number;
  income_Start: number;
  income_End: number;
  rate: number;
  deduction: number;
  update_By: string;
  update_Time: Date;
  is_Disabled: boolean;
}
export interface IncomeTaxBracketSettingMemory {
  param: IncomeTaxBracketSettingParam;
  pagination: Pagination;
  data: IncomeTaxBracketSettingMain[];
  selectedData: IncomeTaxBracketSettingDto;
}
export interface IncomeTaxBracketSettingDto extends IncomeTaxBracketSettingMain{
  subData: IncomeTaxBracketSetting_SubData[]
}
export interface IncomeTaxBracketSetting_SubData {
  tax_Level: number;
  income_Start: number;
  income_End: number;
  rate: number;
  deduction: number;
  update_By: string;
  update_Time: Date;
  update_Time_Str: string;
  is_Duplicate: boolean;
}
