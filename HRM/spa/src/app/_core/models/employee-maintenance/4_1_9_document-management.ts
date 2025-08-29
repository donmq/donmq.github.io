import { Pagination } from "@utilities/pagination-utility";

export interface DocumentManagement_MainParam {
  division: string;
  factory: string;
  employee_Id: string;
  local_Full_Name: string;
  document_Type: string;
  validity_Date_Start_From: string;
  validity_Date_Start_To: string;
  validity_Date_Start_From_Str: string;
  validity_Date_Start_To_Str: string;
  validity_Date_End_From: string;
  validity_Date_End_To: string;
  validity_Date_End_From_Str: string;
  validity_Date_End_To_Str: string;
  lang: string;

}
export interface DocumentManagement_MainData {
  division: string;
  factory: string;
  employee_Id: string;
  local_Full_Name: string;
  document_Type: string;
  document_Type_Name: string;
  passport_Full_Name: string;
  seq: number;
  document_Number: string;
  validity_Date_From: Date;
  validity_Date_To: Date;
  update_By: string;
  update_Time: Date
  file_List: DocumentManagement_FileModel[]
  ser_Num: string
}
export interface DocumentManagement_SubParam {
  division: string;
  factory: string;
  employee_Id: string;
  local_Full_Name: string;
  max_Seq: number;
}
export interface DocumentManagement_SubData {
  document_Type: string;
  passport_Full_Name: string
  seq: number
  document_Number: string
  validity_Date_From: Date;
  validity_Date_From_Str: string;
  validity_Date_To: Date;
  validity_Date_To_Str: string;
  update_By: string;
  update_Time: Date
  update_Time_Str: string
  file_List: DocumentManagement_FileModel[]
  ser_Num: string
}
export interface DocumentManagement_MainMemory {
  param: DocumentManagement_MainParam;
  pagination: Pagination
  data: DocumentManagement_MainData[];
}
export interface DocumentManagement_SubMemory {
  param: DocumentManagement_SubParam;
  data: DocumentManagement_SubData[];
}
export interface DocumentManagement_ModalInputModel {
  division: string;
  factory: string;
  ser_Num: string;
  employee_Id: string;
  file_List: DocumentManagement_FileModel[]
}
export interface DocumentManagement_FileModel {
  id: number;
  name: string;
  content?: string;
  size: number;
}
export interface DocumentManagement_DownloadFileModel {
  division: string;
  factory: string;
  ser_Num: string;
  employee_Id: string;
  file_Name: string;
}
export interface DocumentManagement_SubModel {
  division: string;
  factory: string;
  employee_Id: string;
  document_Type: string;
  seq: number;
}
export interface DocumentManagement_TypeheadKeyValue {
  key: string;
  useR_GUID: string;
  local_Full_Name: string;
  max_Seq: number;
}
