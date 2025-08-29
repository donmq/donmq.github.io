import { Pagination } from "@utilities/pagination-utility";

export interface EmpInfo {
  useR_GUID: string;
  local_Full_Name: string;
  department_Code: string;
  factory: string;
  work_Shift_Type: string;
  work8hours: string;
  work_Type: string;
  special_Regular_Work_Type: string;
  employment_Status: string
}

export interface PregnancyMaternityDetail {
  seq: number;
  factory: string;
  employee_ID: string;
  useR_GUID: string;
  local_Full_Name: string;
  department_Code: string;
  department_Name: string;
  department_Code_Name: string;
  work_Shift_Type: string;
  work_Shift_Type_Name: string;
  due_Date: Date;
  due_Date_Str: string;
  work8hours: boolean | null;
  work8hours_Str: string;
  work7hours: Date | null;
  work7hours_Str: string;
  pregnancy36Weeks: Date | null;
  pregnancy36Weeks_Str: string;
  maternity_Start: Date | null;
  maternity_Start_Str: string;
  maternity_End: Date | null;
  maternity_End_Str: string;
  goWork_Date: Date | null;
  goWork_Date_Str: string;
  close_Case: boolean | null;
  close_Case_Str: string;
  work_Type_Before: string;
  work_Type_After: string;
  pregnancy_Week: string;
  special_Regular_Work_Type: string;
  remark: string;
  ultrasound_Date: Date | null;
  ultrasound_Date_Str: string;
  baby_Start: Date | null;
  baby_Start_Str: string;
  baby_End: Date | null;
  baby_End_Str: string;
  estimated_Date1: Date | null;
  estimated_Date1_Str: string;
  estimated_Date2: Date | null;
  estimated_Date2_Str: string;
  estimated_Date3: Date | null;
  estimated_Date3_Str: string;
  estimated_Date4: Date | null;
  estimated_Date4_Str: string;
  estimated_Date5: Date | null;
  estimated_Date5_Str: string;
  insurance_Date1: Date | null;
  insurance_Date1_Str: string;
  insurance_Date2: Date | null;
  insurance_Date2_Str: string;
  insurance_Date3: Date | null;
  insurance_Date3_Str: string;
  insurance_Date4: Date | null;
  insurance_Date4_Str: string;
  insurance_Date5: Date | null;
  insurance_Date5_Str: string;
  leave_Date1: Date | null;
  leave_Date1_Str: string;
  leave_Date2: Date | null;
  leave_Date2_Str: string;
  leave_Date3: Date | null;
  leave_Date3_Str: string;
  leave_Date4: Date | null;
  leave_Date4_Str: string;
  leave_Date5: Date | null;
  leave_Date5_Str: string;
  update_By: string;
  update_Time: Date;
  update_Time_Str: string;
}

export interface PregnancyMaternityParam {
  factory: string;
  department_Code: string;
  employee_ID: string;
  dueDate_Start: string;
  dueDate_Start_Str: string;
  dueDate_End: string;
  dueDate_End_Str: string;
  maternityLeave_Start: string;
  maternityLeave_Start_Str: string;
  maternityLeave_End: string;
  maternityLeave_End_Str: string;
  language: string;
}

export interface PregnancyMaternityMemory {
  params: PregnancyMaternityParam;
  pagination: Pagination;
  datas: PregnancyMaternityDetail[];
  selectedData: PregnancyMaternityDetail
}
export interface PregnancyMaternity_TypeheadKeyValue {
  key: string;
  useR_GUID: string;
}
