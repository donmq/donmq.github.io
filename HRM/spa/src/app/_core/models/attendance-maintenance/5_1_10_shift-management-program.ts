import { Pagination } from "@utilities/pagination-utility";

export interface ShiftManagementProgram_Param {
  division: string;
  useR_GUID: string;
  factory: string;
  department: string;
  employee_Id: string;
  work_Shift_Type_New: string;
  effective_Date: string;
  effective_Date_Str: string;
  lang: string;
}
export interface ShiftManagementProgram_Main {
  division: string;
  factory: string;
  useR_GUID: string;
  employee_Id: string;
  local_Full_Name: string;
  department: string;
  department_Name: string;
  work_Shift_Type_Old: string;
  work_Shift_Type_Old_Name: string;
  work_Shift_Type_New: string;
  work_Shift_Type_New_Name: string;
  effective_Date: string;
  effective_Date_Str: string;
  effective_State: boolean;
  update_By: string;
  update_Time: Date;
  update_Time_Str: string;
  isChecked: boolean
  is_Editable: boolean
  is_Duplicate: boolean
}
export interface ShiftManagementProgram_MainMemory {
  param: ShiftManagementProgram_Param
  pagination: Pagination
  data: ShiftManagementProgram_Main[]
}
export interface ShiftManagementProgram_Update {
  temp_Data: ShiftManagementProgram_Main
  recent_Data: ShiftManagementProgram_Main
}
export interface TypeheadKeyValue {
  key: string;
  useR_GUID: string;
  name: string;
  department: string;
  work_Shift_Type_Old: string;
}
