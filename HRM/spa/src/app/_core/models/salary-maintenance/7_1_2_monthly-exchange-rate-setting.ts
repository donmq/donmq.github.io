import { Pagination } from '../../utilities/pagination-utility';

export interface MonthlyExchangeRateSetting_Param {
  rate_Month: string;
  rate_Month_Str: string;
  lang: string;
}
export interface MonthlyExchangeRateSetting_Main {
  rate_Month: string;
  rate_Month_Str: string;
  kind: string;
  kind_Name: string;
  currency: string;
  currency_Name: string;
  exchange_Currency: string;
  exchange_Currency_Name: string;
  rate: string;
  rate_Date: string;
  rate_Date_Str: string;
  update_By: string;
  update_Time: string;
  update_Time_Str: string;
  is_Duplicate: boolean
}
export interface MonthlyExchangeRateSetting_Update {
  param: MonthlyExchangeRateSetting_Param
  data: MonthlyExchangeRateSetting_Main[]
}
export interface MonthlyExchangeRateSetting_Memory {
  param: MonthlyExchangeRateSetting_Param
  pagination: Pagination
  selectedData: MonthlyExchangeRateSetting_Update
  data: MonthlyExchangeRateSetting_Main[]
  tempUrl: string
}
