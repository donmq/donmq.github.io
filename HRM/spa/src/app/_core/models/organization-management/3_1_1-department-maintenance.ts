import { Pagination } from "@utilities/pagination-utility";

export interface HRMS_Org_Department {
  division: string;
  factory: string;
  center_Code: string;
  org_Level: string;
  org_Level_Name: string | null;
  department_Code: string;
  department_Name: string;
  department_Name_Lang: string;
  upper_Department: string;
  upper_Department_Name: string | null;
  attribute: string;
  virtual_Department: string;
  virtual_Department_Name: string | null;
  isActive: boolean;
  supervisor_Employee_ID: string;
  supervisor_Type: string;
  approved_Headcount: number | null;
  cost_Center: string;
  effective_Date: string;
  expiration_Date: string | null;
  update_By: string;
  update_Time: string;
  status: string;
}

export interface HRMS_Org_Department_Param {
  status: string
  division: string;
  factory: string;
  department_Code: string;
  department_Name: string;
  lang: string;
}

export interface Language {
  division: string;
  factory: string;
  department_Code: string;
  department_Name: string;
  detail: LanguageParams[];
  userName: string;
}

export interface LanguageParams {
  language_Code: string;
  department_Name: string;
}

export interface languageSource {
  callBack: boolean;
  data: Language;
}

export interface HRMS_Org_DepartmentParamSource {
  currentPage: Pagination;
  status: boolean;
  selectedData: HRMS_Org_Department
  dataMain: HRMS_Org_Department[]
  param: HRMS_Org_Department_Param
}

export interface ListUpperVirtual {
  department: string;
  departmentName: string;
}
