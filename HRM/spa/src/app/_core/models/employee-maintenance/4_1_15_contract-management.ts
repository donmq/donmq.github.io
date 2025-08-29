import { Pagination } from "@utilities/pagination-utility";

export interface ContractManagementDto {
  division: string;
  factory: string;
  employee_ID: string;
  seq: number;
  contract_Type: string;
  contract_Start: string;
  contract_End: string;
  effective_Status: boolean;
  probation_Start: string;
  probation_End: string;
  assessment_Result: string;
  assessment_Result_Name: string;
  extend_to: string;
  reason: string;
  update_By: string;
  update_Time: Date;
  local_Full_Name: string;
  department: string;
  onboard_Date: Date;
  contract_Title: string;
}

export interface ContractManagementParam {
  division: string;
  factory: string;
  employeeID: string;
  localFullName: string;
  department: string;
  contractType: string;
  effectiveStatus: string;
  onboard_Date_From: string;
  onboard_Date_From_Date: string;
  onboard_Date_To: string;
  onboard_Date_To_Date: string;
  contract_End_From: string;
  contract_End_From_Date: string;
  contract_End_To: string;
  contract_End_To_Date: string;
  probation_End_From: string;
  probation_End_From_Date: string;
  probation_End_To: string;
  probation_End_To_Date: string;
}

export interface ProbationParam {
  probationary_Period: boolean;
  probationary_Year: number | null;
  probationary_Month: number | null;
  probationary_Day: number | null;
}

export interface Personal {
  local_Full_Name: string;
  nationality: string;
  department: string;
  seq: number[];
  onboard_Date: Date | null;
}

export interface PersonalParam {
  division: string;
  factory: string;
  employeeID: string;
  lang: string;
}

export interface Contract_ManagementParamSource {
  param: ContractManagementParam;
  dataMain: ContractManagementDto[];
  currentPage: Pagination;
}
