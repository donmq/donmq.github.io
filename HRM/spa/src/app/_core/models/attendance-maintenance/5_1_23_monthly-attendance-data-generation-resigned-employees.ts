export interface GenerationResignedParam {
  factory: string;
  att_Month: string;
  resign_Date: string;
  employee_ID_Start: string;
  employee_ID_End: string;
}
export interface GenerationResigned extends GenerationResignedParam {
  working_Days: number;
  is_Delete: boolean;
}

export interface ResignedMonthlyDataCloseParam{
  factory: string;
  att_Month: string;
  pass: string;
}

