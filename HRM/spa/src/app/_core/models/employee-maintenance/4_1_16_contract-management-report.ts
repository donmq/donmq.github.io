import { Pagination } from "@utilities/pagination-utility";

export interface ContractManagementReportDto {
  seq: number;
  contract_Type: string;
  contract_Type_Name: string;
  document_Type: string;
  document_Type_Name: string;
  salary_Type: string;
  department: string;
  department_Name: string;
  employee_ID: string;
  local_Full_Name: string;
  contract_Start: string;
  contract_End: string;
  onboard_Date: Date | null;
  probation_Start: string;
  probation_End: string;
  assessment_Result: string;
  extend_To: Date | null;
  reason: string;
}

export interface ContractManagementReportParam {
  division: string;
  factory: string;
  contract_Type: string;
  salary_Type: string;
  document_Type: string;
  document_Type_Name: string;
  onboard_Date_From: string;
  onboard_Date_From_Date: string;

  onboard_Date_To: string;
  onboard_Date_To_Date: string;

  contract_End_From: string;
  contract_End_From_Date: string;

  contract_End_To: string;
  contract_End_To_Date: string;

  department_From: string;
  department_To: string;
  department: string[];
  employee_ID_From: string;
  employee_ID_To: string;
  lang: string;
}

export interface Contract_Management_ReportParamSource {
  param: ContractManagementReportParam;
  dataMain: ContractManagementReportDto[];
  currentPage: Pagination;
}
