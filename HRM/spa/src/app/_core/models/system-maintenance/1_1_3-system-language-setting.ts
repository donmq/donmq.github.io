import { Pagination } from "@utilities/pagination-utility";

export interface SystemLanguageSetting_Data {
  language_Code: string;
  language_Name: string;
  isActive: boolean;
}

export interface SystemLanguageSetting_Memory {
  pagination: Pagination;
  selectedData: SystemLanguageSetting_Data
}
