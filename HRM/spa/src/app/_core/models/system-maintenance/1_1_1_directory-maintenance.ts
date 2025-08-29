import { Pagination } from "@utilities/pagination-utility";

export interface DirectoryMaintenance_Data {
  seq: string;
  directory_Code: string;
  directory_Name: string;
  parent_Directory_Code: string;
  language: string;
  update_By: string;
  update_Time: string | null;
}

export interface DirectoryMaintenance_Param {
  directory_Code: string;
  directory_Name: string;
  parent_Directory_Code: string;
}

export interface  DirectoryMaintenance_Memory {
  pagination: Pagination;
  param: DirectoryMaintenance_Param,
  selectedData: DirectoryMaintenance_Data,
  data: DirectoryMaintenance_Data[]
}
