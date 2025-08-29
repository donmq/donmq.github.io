import { Pagination } from '@utilities/pagination-utility';

export interface AdditionDeductionItemAndAmountSettings_MainData {
  factory: string;
  permission_Group: string;
  permission_Group_Title: string;
  salary_Type: string;
  salary_Type_Title: string;
  effective_Month: Date;
  effective_Month_Str: string;
  addDed_Type: string;
  addDed_Type_Title: string;
  addDed_Item: string;
  addDed_Item_Title: string;
  amount: number;
  onjob_Print: string;
  resigned_Print: string;
  update_By: string;
  update_Time: Date;
  update_Time_Str: string;
  isDisable: boolean;
}

export interface AdditionDeductionItemAndAmountSettings_SubData {
  addDed_Type: string;
  addDed_Item: string;
  amount: number;
  onjob_Print: string;
  resigned_Print: string;
  update_By: string;
  update_Time: Date;
  update_Time_Str: string;
  is_Duplicate: boolean;
  isDisable: boolean;
  isNew: boolean;
}
export interface AdditionDeductionItemAndAmountSettings_MainParam {
  factory: string;
  permission_Group: string[];
  salary_Type: string;
  addDed_Type: string;
  addDed_Item: string;
  effective_Month: string;
  onjob_Print: string;
  resigned_Print: string;
  language: string;
}
export interface AdditionDeductionItemAndAmountSettings_SubParam {
  factory: string;
  permission_Group: string;
  salary_Type: string;
  effective_Month: Date;
  effective_Month_Str: string;
}
export interface AdditionDeductionItemAndAmountSettings_MainMemory {
  param: AdditionDeductionItemAndAmountSettings_MainParam;
  pagination: Pagination;
  data: AdditionDeductionItemAndAmountSettings_MainData[];
  selectedData: AdditionDeductionItemAndAmountSettings_MainData;
}

export interface AdditionDeductionItemAndAmountSettings_SubMemory {
  data: AdditionDeductionItemAndAmountSettings_SubData[];
  param: AdditionDeductionItemAndAmountSettings_SubParam;
}
