export interface DailySwipeCardAnomaliesList_Param {
  factory: string
  department: string
  employee_Id: string;
  clock_Out_Date_From: string;
  clock_Out_Date_To: string;
  clock_Out_Date_From_Str: string;
  clock_Out_Date_To_Str: string;
  lang: string
}
export interface DailySwipeCardAnomaliesList_Memory {
  param: DailySwipeCardAnomaliesList_Param
  total: number
}
