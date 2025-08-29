export interface LoanedDataGeneration_Param   {
    factory: string;
    loaned_Year_Month_Str: string;
    loaned_Date_Start_Str: string;
    loaned_Date_End_Str: string;
    normal_Working_Days: number;
    employee_ID_Start: string;
    employee_ID_End: string;
    selected_Tab : string;
    close_Status: string
    language: string;
}
export interface  LoanedDataGeneration_Base {
  param : LoanedDataGeneration_Param,
  loaned_Year_Month: Date;
  loaned_Date_Start: Date;
  loaned_Date_End: Date;
}
export interface LoanedDataGeneration_Memory {
  data: LoanedDataGeneration_Base
}
