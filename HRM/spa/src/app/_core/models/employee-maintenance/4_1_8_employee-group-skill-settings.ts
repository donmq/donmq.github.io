import { Pagination } from "@utilities/pagination-utility";

export interface EmployeeGroupSkillSettings_Param {
  division: string;
  factory: string;
  employee_Id: string;
  local_Full_Name: string;
  production_Line: string;
  performance_Category: string;
  technical_Type: string;
  expertise_Category: string;
  skill_Array: string[];
  lang: string;

}
export interface EmployeeGroupSkillSettings_Main {
  division: string;
  factory: string;
  employee_Id: string;
  local_Full_Name: string;
  production_Line: string;
  production_Line_Name: string;
  performance_Category: string;
  performance_Category_Name: string;
  technical_Type: string;
  technical_Type_Name: string;
  expertise_Category: string;
  expertise_Category_Name: string;
  skill_Detail_List: EmployeeGroupSkillSettings_SkillDetail[]
}
export interface EmployeeGroupSkillSettings_SkillDetail {
  seq: string;
  skill_Certification: string;
  passing_Date: Date | null;
  passing_Date_Str: string;

}
export interface EmployeeGroupSkillSettings_MainMemory {
  param: EmployeeGroupSkillSettings_Param
  pagination : Pagination
  data: EmployeeGroupSkillSettings_Main[]
}
