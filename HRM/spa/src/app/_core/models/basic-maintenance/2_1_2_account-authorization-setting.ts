import { Pagination } from "@utilities/pagination-utility";

export interface AccountAuthorizationSetting_Base {
  account: string;
  name: string;
  division: string;
  factory: string;
  department_ID: string;
  listRole_Str: string;
  listRole: string[];
}
export interface AccountAuthorizationSetting_Param extends AccountAuthorizationSetting_Base {
  isActive: number;
  lang: string
}
export interface AccountAuthorizationSetting_Data extends AccountAuthorizationSetting_Base {
  isActive: boolean;
  password_Reset: boolean;
  update_By: string;
  update_Time: Date;
}
export interface AccountAuthorizationSetting_Memory {
  param: AccountAuthorizationSetting_Param
  pagination: Pagination
  selectedData: AccountAuthorizationSetting_Data
  data: AccountAuthorizationSetting_Data[]
}
