import { Pagination } from "@utilities/pagination-utility";

export interface SalaryAdjustmentMaintenanceMain {
  history_GUID: string;
  useR_GUID: string;
  division: string;
  factory: string;
  employee_ID: string;
  local_Full_Name: string;
  onboard_Date: string;
  onboard_Date_Str: string;
  employment_Status: string;
  reason_For_Change: string;
  reason_For_Change_Name: string;
  effective_Date: string;
  effective_Date_Str: string;
  seq: number;
  department: string;
  department_Name: string;
  technical_Type: string;
  expertise_Category: string;
  period_of_Acting_Position: string;
  acting_Position_Start: string;
  acting_Position_End: string;
  position_Grade: number;
  position_Title: string;
  permission_Group: string;
  update_By: string;
  update_Time: string;
  salary_Type: string;
  salary_Grade: number;
  salary_Level: number;
  currency: string;
  probation_Salary_Month: string;
  before: HistoryDetail;
  after: HistoryDetail;
  salary_Item: SalaryAdjustmentMaintenance_SalaryItem[];
}
export interface HistoryDetail {
  department: string;
  department_Name: string;
  position_Grade: number;
  position_Title: string;
  salary_Type: string;
  permission_Group: string;
  technical_Type: string;
  expertise_Category: string;
  acting_Position_Start: string;
  acting_Position_End: string;
  salary_Grade: number;
  salary_Level: number;
  item: SalaryAdjustmentMaintenance_SalaryItem[];
}
export interface SalaryAdjustmentMaintenance_SalaryItem {
  salary_Item: string;
  salary_Item_Name: string;
  amount: number;
}
export interface CheckEffectiveDateResult {
  checkEffectiveDate: boolean;
  maxSeq: number;
}
export interface SalaryAdjustmentMaintenance_PersonalDetail {
  useR_GUID: string;
  division: string;
  local_Full_Name: string;
  onboard_Date: string;
  resign_Date: string;
  employment_Status: string;
  before: HistoryDetail;
  after: HistoryDetail;
}
export interface SalaryAdjustmentMaintenanceParam {
  factory: string;
  department: string;
  employee_ID: string;
  onboard_Date: string;
  reason_For_Change: string;
  effective_Date_Start: string;
  effective_Date_End: string;
  lang: string;
}
export interface SalaryAdjustmentMaintenanceSource {
  selectedData: SalaryAdjustmentMaintenanceMain;
  paramQuery: SalaryAdjustmentMaintenanceParam;
  dataMain: SalaryAdjustmentMaintenanceMain[];
  pagination: Pagination;
}
