import { Pagination } from '@utilities/pagination-utility';

export interface AdditionDeductionItemToAccountingCodeMappingMaintenanceDto {
  factory: string;
  addDed_Item: string;
  addDed_Item_Title: string;
  main_Acc: string;
  sub_Acc: string;
  update_By: string;
  update_Time: Date;
  update_Time_Str: string;
}

export interface AdditionDeductionItemToAccountingCodeMappingMaintenanceParam {
  factory: string;
  language: string;
}
export interface AdditionDeductionItemToAccountingCodeMappingMaintenanceMemory {
  param: AdditionDeductionItemToAccountingCodeMappingMaintenanceParam;
  pagination: Pagination;
  data: AdditionDeductionItemToAccountingCodeMappingMaintenanceDto[];
  selectedData: AdditionDeductionItemToAccountingCodeMappingMaintenanceDto;
}
