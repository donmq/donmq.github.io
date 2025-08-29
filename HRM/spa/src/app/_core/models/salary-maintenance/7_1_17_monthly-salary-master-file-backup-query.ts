import { Pagination } from '@utilities/pagination-utility';

export interface MonthlySalaryMasterFileBackupQueryParam {
  year_Month: string;
  year_Month_Str: string;
  factory: string;
  department: string;
  employee_ID: string;
  employment_Status: string;
  position_Title: string;
  permission_Group: string[];
  salary_Type: string;
  salary_Grade: string;
  salary_Level: string;
  lang: string;
}

export interface MonthlySalaryMasterFileBackupQueryDto {
  user_GUID: string;
  probation: string;
  yearMonth: string;
  factory: string;
  department: string;
  employee_ID: string;
  local_Full_Name: string;
  employment_Status: string;
  position_Grade: string;
  position_Title: string;
  actingPosition_Start: string;
  actingPosition_End: string;
  technical_Type: string;
  expertise_Category: string;
  onboard_Date: string;
  permission_Group: string;
  salary_Type: string;
  salary_Grade: string;
  salary_Level: string;
  currency: string;
  total_Salary: string;
  update_By: string;
  update_Time: string;
}

export interface SalaryDetailDto {
  salary_Item: string;
  amount: number;
}

export interface MonthlySalaryMasterFileBackupQuerySource {
  salarySearch_Param: MonthlySalaryMasterFileBackupQueryParam;
  batchData_Param: MonthlySalaryMasterFileBackupQueryParam;
  salarySearch_Data: MonthlySalaryMasterFileBackupQueryDto[];
  pagination: Pagination;
  selectedData: MonthlySalaryMasterFileBackupQueryDto;
  selected_Tab: string;
}
