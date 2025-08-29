import { Pagination } from "@utilities/pagination-utility";

export interface HRMS_Basic_Code_TypeDto {
  type_Seq: string;
  type_Name: string;
  update_By: string;
  update_Time: string | null;
  info: Info[];

}
export interface HRMS_Basic_Code_TypeParam {
  type_Seq: string;
  type_Name: string;
}

export interface ResultMain {
  type_Seq: string;
  type_Name: string;
  info: Info[];
}
export interface Info {
  type_Seq: string;
  type_Name: string;
  language_Code: string;
}

//signal
export interface HRMS_Type_Code_Source {
  currentPage: number;
  isEdit: boolean;
  status: boolean;
  source?: HRMS_Basic_Code_TypeDto
}
export interface BasicCode {
  param: HRMS_Basic_Code_TypeParam
  pagination : Pagination
  data: HRMS_Basic_Code_TypeDto[]
}


export interface Language_Dto {
  type_Seq: string;
  detail_Dto: LanguageDetail_Dto[];
  userName: string;
}

export interface LanguageDetail_Dto {
  language_Code: string;
  type_Name: string;
}
export interface languageDto {
  type_Seq: string;
  type_Name: string;
}


export interface languageSource {
  isEdit: boolean;
  status: boolean;
  source?: languageDto

}
