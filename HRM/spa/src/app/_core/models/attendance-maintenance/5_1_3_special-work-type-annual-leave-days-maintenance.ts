import { Pagination } from "@utilities/pagination-utility";

export interface HRMS_Att_Work_Type_DaysDto {
  division: string;
  factory: string;
  work_Type: string;
  annual_leave_days: string;
  effective_State: boolean;
  update_By: string;
  update_Time: Date | string;
}

export interface SpecialWorkTypeAnnualLeaveDaysMaintenanceParam {
  division: string;
  factory: string;
  lang: string;
}

export interface UpdateEffective_StateParam {
  division: string;
  factory: string;
  effective_State: boolean;
  work_Type: string;
}

export interface SpecialWorkTypeAnnualLeaveDaysMaintenanceSource {
  param: SpecialWorkTypeAnnualLeaveDaysMaintenanceParam
  pagination: Pagination
  data: HRMS_Att_Work_Type_DaysDto
}
