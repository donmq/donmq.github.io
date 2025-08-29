import { Pagination } from "@utilities/pagination-utility";

export interface HRMS_Emp_Identity_Card_HistoryDto {
  nationality_Before: string;
  identification_Number_Before: string;
  local_Full_Name: string;
  issued_Date_Before: string;
  nationality_After: string;
  identification_Number_After: string;
  issued_Date_After: string;
  update_By_After: string;
  update_By_Before: string;
  update_By: string;
  update_Time: string;
  user_GUID: string;
  history_GUID: string;
}

export interface HRMS_Emp_Identity_Card_HistoryParam {
  nationality: string;
  identification_Number: string;
}

export interface HRMS_Emp_Identity_Card_History_Source {
  currentPage: number,
  param: HRMS_Emp_Identity_Card_HistoryParam,
  itemDetail: HRMS_Emp_Identity_Card_HistoryDto,
  isAdd: boolean;
  basicCode: HRMS_Emp_Identity_Card_HistoryDto
}

export interface HRMS_Emp_PersonalView {
  nationality: string;
  identification_Number: string;
  issued_Date: string;
}
export interface IdentificationCardHistory {
  param: HRMS_Emp_Identity_Card_HistoryParam
  pagination : Pagination
  data: HRMS_Emp_Identity_Card_HistoryDto[]
}
