import { Pagination } from "@utilities/pagination-utility";

export interface Certifications_MainParam {
  division: string;
  factory: string;
  employee_Id: string;
  local_Full_Name: string;
  category_Of_Certification: string;
  passing_Date_From: string;
  passing_Date_To: string;
  passing_Date_From_Str: string;
  passing_Date_To_Str: string;
  certification_Valid_Period_From: string;
  certification_Valid_Period_To: string;
  certification_Valid_Period_From_Str: string;
  certification_Valid_Period_To_Str: string;
  lang: string;

}
export interface Certifications_MainData {
  division: string;
  factory: string;
  employee_Id: string;
  local_Full_Name: string;
  seq: number
  category_Of_Certification: string;
  name_Of_Certification: string;
  score: string;
  level: string;
  result: boolean;
  passing_Date: Date;
  certification_Valid_Period: Date | null;
  file_List: Certifications_FileModel[]
  remark: string
  update_By: string;
  update_Time: Date
  ser_Num: string
}
export interface Certifications_SubParam {
  division: string;
  factory: string;
  employee_Id: string;
  local_Full_Name: string;
  max_Seq: number;
}
export interface Certifications_SubData {
  seq: number
  category_Of_Certification: string;
  name_Of_Certification: string;
  score: string;
  level: string;
  result: boolean;
  passing_Date: Date;
  passing_Date_Str: string;
  certification_Valid_Period: Date | null;
  certification_Valid_Period_Str: string;
  file_List: Certifications_FileModel[]
  remark: string
  update_By: string;
  update_Time: Date
  update_Time_Str: string
  ser_Num: string
}
export interface Certifications_MainMemory {
  param: Certifications_MainParam;
  pagination: Pagination
  data: Certifications_MainData[];
}
export interface Certifications_SubMemory {
  param: Certifications_SubParam;
  data: Certifications_SubData[];
}
export interface Certifications_ModalInputModel {
  division: string;
  factory: string;
  ser_Num: string;
  employee_Id: string;
  file_List: Certifications_FileModel[]
}
export interface Certifications_FileModel {
  id: number;
  name: string;
  content?: string;
  size: number;
}
export interface Certifications_DownloadFileModel {
  division: string;
  factory: string;
  ser_Num: string;
  employee_Id: string;
  file_Name: string;
}
export interface Certifications_SubModel {
  division: string;
  factory: string;
  employee_Id: string;
  seq: number;
}
export interface Certifications_TypeheadKeyValue {
  key: string;
  useR_GUID: string;
  local_Full_Name: string;
  max_Seq: number;
}
