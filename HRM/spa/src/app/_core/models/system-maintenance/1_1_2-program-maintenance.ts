import { Pagination } from "@utilities/pagination-utility";

export interface ProgramMaintenance_Data {
  seq: number
  program_Code: string;
  program_Name: string;
  parent_Directory_Code: string;
  update_By: string;
  update_Time: string | null;
  functions: string[];
  functions_Str: string;
}
export interface ProgramMaintenance_Param {
  program_Code: string;
  program_Name: string;
  parent_Directory_Code: string;
}
export interface ProgramMaintenance_Memory {
  param: ProgramMaintenance_Param;
  pagination: Pagination;
  selectedData: ProgramMaintenance_Data
  data: ProgramMaintenance_Data[]
}
