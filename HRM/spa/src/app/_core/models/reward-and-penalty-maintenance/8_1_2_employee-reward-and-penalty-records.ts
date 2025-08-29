import { Pagination } from "@utilities/pagination-utility";

export interface D_8_1_2_EmployeeRewardPenaltyRecordsParam {
  factory: string;
  department: string;
  employee_ID: string;
  date_Start: string;
  date_End: string;
  yearly_Month_Start: string;
  yearly_Month_End: string;
  date_Start_Str: string;
  date_End_Str: string;
  yearly_Month_Start_Str: string;
  yearly_Month_End_Str: string;
  language: string;
}

export interface D_8_1_2_EmployeeRewardPenaltyRecordsData {
  history_GUID: string;
  factory: string;
  division: string;
  useR_GUID: string;
  employee_ID: string;
  local_Full_Name: string;
  department_Code: string;
  department_Code_Name: string;
  work_Type: string;
  work_Type_Name: string;
  reward_Date: Date;
  reward_Date_Str: string;
  reward_Penalty_Type: string;
  reward_Penalty_Type_Name: string;
  reason_Code: string;
  reason_Code_Name: string;
  yearly_Month: Date;
  yearly_Month_Str: string;
  counts_of: number;
  remark: string;
  serNum: string;
  update_By: string;
  update_Time: string;
}
export interface D_8_1_2_EmployeeRewardPenaltyRecordsSubParam extends D_8_1_2_EmployeeRewardPenaltyRecordsData {
  file_List: EmployeeRewardPenaltyRecordsReportFileModel[];
}

export interface D_8_1_2_EmployeeRewardPenaltyRecordsAttachment {
  division: string;
  factory: string;
  program_Code: string;
  serNum: string;
  fileID: string;
  fileName: string;
  fileSize: string;
  update_By: string;
  update_Time: Date;
}

export interface D_8_1_2_EmployeeRewardPenaltyRecordsReport {
  factory: string;
  employee_ID: string;
  date: Date;
  reward_Penalty_Type: string;
  reason_Code: string;
  yearly_Month?: Date | null;
  counts_of: number;
  remark: string;
  isCorrect: string;
  error_Message: string;
}
export interface EmployeeRewardPenaltyRecordsReportDownloadFileModel {
  division: string;
  factory: string;
  serNum: string;
  employee_ID: string;
  file_Name: string;
}
export interface EmployeeRewardAndPenaltyRecords_ModalInputModel {
  division: string;
  factory: string;
  serNum: string;
  employee_ID: string;
  file_List: EmployeeRewardPenaltyRecordsReportFileModel[]
}
export interface EmployeeRewardPenaltyRecordsReportFileModel {
  id: number;
  name: string;
  content?: string;
  size: number;
}
export interface EmployeeRewardAndPenaltyRecords_Memory {
  isEdit: boolean;
  isQuery: boolean;
  isAdd: boolean;
  param: D_8_1_2_EmployeeRewardPenaltyRecordsParam;
  pagination: Pagination
  selectedData: D_8_1_2_EmployeeRewardPenaltyRecordsSubParam
  data: D_8_1_2_EmployeeRewardPenaltyRecordsData[]
}
export interface EmployeeRewardAndPenaltyRecords_TypeheadKeyValue {
  key: string;
  uSER_GUID: string;
  local_Full_Name: string;
  department: string;
  work_Shift_Type: string;
}
