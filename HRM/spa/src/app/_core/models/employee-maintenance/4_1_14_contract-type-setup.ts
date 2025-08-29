import { Pagination } from "@utilities/pagination-utility";

export interface ContractTypeSetupDto {
  division: string;
  factory: string;
  contract_Type: string;
  contract_Type_After: string;
  contract_Title: string;
  probationary_Period: boolean;
  probationary_Period_Str: string;
  probationary_Year: number;
  probationary_Month: number;
  probationary_Day: number;
  alert: boolean;
  alert_Str: string;
  seq: number;
  schedule_Frequency: string;
  day_Of_Month: number;
  alert_Rules: string;
  days_Before_Expiry_Date: number;
  month_Range: number;
  contract_Start: number;
  contract_End: number;
  update_By: string;
  update_Time: Date;
  dataDetail: HRMSEmpContractTypeDetail[];
  lang: string;
}

export interface ContractTypeSetup_MainMemory {
  param: ContractTypeSetupParam;
  pagination: Pagination;
  data: ContractTypeSetupDto[];
}

export interface ContractTypeSetupSource {
  source?: ContractTypeSetupDto;
  paramQuery: ContractTypeSetupParam;
  pagination: Pagination;
}

export interface HRMSEmpContractTypeDetail {
  division: string;
  factory: string;
  contract_Type: string;
  seq: number;
  schedule_Frequency: string;
  day_Of_Month: number;
  alert_Rules: string;
  days_Before_Expiry_Date: number;
  month_Range: number;
  contract_Start: number;
  contract_End: number;
  update_By: string;
  update_Time: Date;
  probationary_Period: boolean;
  alert: boolean;
}
export interface ContractTypeSetupParam {
  division: string;
  factory: string;
  contract_Type: string;
  alert_Str: string;
  probationary_Period_Str: string;
  lang: string;
}
