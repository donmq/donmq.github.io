import { Pagination } from "@utilities/pagination-utility";

export interface ContributionRateSettingDto {
  factory: string;
  effective_Month: Date | string;
  effective_Month_Str: string;
  permission_Group: string;
  permission_Group_Name: string;
  insurance_Type: string;
  insurance_Type_Name: string;
  employer_Rate: string;
  employee_Rate: string;
  update_By: string;
  update_Time: string;
}
export interface ContributionRateSettingParam {
  factory: string;
  effective_Month: string;
  effective_Month_Str: string;
  permission_Group: string[];
  language: string;
}

export interface ContributionRateSettingSubParam {
  factory: string;
  effective_Month: Date;
  effective_Month_Str: string;
  permission_Group: string;
  insurance_Type: string;
}

export interface ContributionRateSettingSubData {
  insurance_Type: string;
  insurance_Type_Name: string;
  employer_Rate: number;
  employee_Rate: number;
  update_By: string;
  update_Time: Date;
  update_Time_Str: string;
  isNew: boolean
  isDuplicate: boolean;
  isDisable: boolean;
}

export interface ContributionRateSettingForm {
    param: ContributionRateSettingSubParam;
    subData: ContributionRateSettingSubData[];
}

export interface ContributionRateSettingCheckEffectiveMonth {
    checkEffective_Month: boolean;
    dataDefault: ContributionRateSettingSubData;
}

export interface ContributionRateSettingSource {
  source?: ContributionRateSettingDto;
  paramQuery: ContributionRateSettingParam;
  dataMain: ContributionRateSettingDto[];
  pagination: Pagination;
}
