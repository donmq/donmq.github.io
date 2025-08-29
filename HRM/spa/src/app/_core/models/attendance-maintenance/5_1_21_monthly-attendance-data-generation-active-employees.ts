import { Pagination } from "@utilities/pagination-utility";

export interface GenerationActiveParam {
  factory: string;
  department:string;
  att_Month: string;
  deadline_Start: string;
  deadline_End: string;
  working_Days: number;
  employee_ID_Start:string;
  employee_ID_End:string;
  is_Delete: boolean;
}

export interface SearchAlreadyDeadlineDataParam {
  factory: string;
  att_Month_Start: string;
  att_Month_End: string;
}

export interface SearchAlreadyDeadlineDataMain {
  factory: string;
  att_Month: string;
  deadline_Start: string;
  deadline_End: string;
  update_By: string;
  update_Time: string;
}

export interface SearchAlreadyDeadlineDataSource {
  param: SearchAlreadyDeadlineDataParam;
  pagination: Pagination;
  data: SearchAlreadyDeadlineDataMain[];
}

export interface ActiveMonthlyDataCloseParam{
  factory: string;
  att_Month: string;
  pass: string;
}
