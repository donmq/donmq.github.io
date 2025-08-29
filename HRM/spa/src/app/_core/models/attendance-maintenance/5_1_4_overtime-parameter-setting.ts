import { Pagination } from "@utilities/pagination-utility";

export interface HRMS_Att_Overtime_ParameterParam {
  division: string;
  factory: string;
  work_Shift_Type: string;
  effective_Month: string;
  language: string;
}

export interface HRMS_Att_Overtime_ParameterUploadParam {
  file: File;
}

export interface HRMS_Att_Overtime_ParameterDTO {
  division: string;
  factory: string;
  work_Shift_Type: string;
  work_Shift_Type_Name: string
  effective_Month: string;
  effective_Month_Date: Date;
  overtime_Start: string;
  overtime_Start_Old: string;
  overtime_End: string;
  overtime_Hours: string;
  night_Hours: string;
  update_By: string;
  update_Time: string;
  language: string;
}

export interface HRMS_Att_Overtime_Parameter_Basic {
  param: HRMS_Att_Overtime_ParameterParam
  pagination: Pagination
  data: HRMS_Att_Overtime_ParameterDTO
}

