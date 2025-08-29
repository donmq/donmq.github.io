import { Pagination } from "@utilities/pagination-utility";

export interface DirectWorkTypeAndSectionSettingBase {
  division: string;
  factory: string;
  effective_Date: string;
  work_Type_Code: string;
}
export interface DirectWorkTypeAndSectionSettingParam extends DirectWorkTypeAndSectionSettingBase {
  section_Code: string;
  direct_Section: string;
  update_By: string;
  update_Time: string;
  lang: string;
}
export interface HRMS_Org_Direct_SectionDto extends DirectWorkTypeAndSectionSettingParam {
  section_Code_Name: string;
  work_Type_Code_Name: string;
}

export interface DirectWorkTypeAndSectionSetting {
  param: DirectWorkTypeAndSectionSettingParam
  pagination: Pagination
  data: HRMS_Org_Direct_SectionDto[]
  selectedData: HRMS_Org_Direct_SectionDto
}
