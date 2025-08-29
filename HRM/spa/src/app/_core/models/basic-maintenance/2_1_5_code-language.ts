import { Pagination } from "@utilities/pagination-utility";

export interface Code_Language {
  type_Seq: string;
  type_Name: string;
  code: string;
  code_Name: string;
  state: string;
  update_By: string;
  update_Time: string | null;
}

export interface Code_LanguageDetail {
  type_Seq: string;
  type_Title: string;
  code: string;
  code_Name: string;
  detail: Code_Language_Form[];
  update_By: string;
  update_Time: string | null;
}

export interface Code_LanguageParam {
  type_Seq: string;
  type_Name: string;
  code: string;
  code_Name: string;
  language_Code: string;
}

export interface CodeNameParam {
  type_Seq: string;
  code: string;
  language_Code: string;
}

export interface Code_LanguageSource {
  pagination: Pagination;
  status: boolean;
  param: Code_LanguageParam,
  source?: Code_Language,
  data: Code_Language[]
}

export interface Code_Language_Form {
  language_Code: string;
  name: string;
}
