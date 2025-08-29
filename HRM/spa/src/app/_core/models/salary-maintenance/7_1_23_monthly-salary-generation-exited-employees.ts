
export interface MonthlySalaryGenerationExitedEmployees_Param {
  factory: string;
  employee_Id: string;
  year_Month: string;
  year_Month_Str: string;
  permission_Group: string[];
  lang: string;
  tab_Type: string;
  total_Rows: number;
  salary_Lock: string;
  salary_Days: number;
}

export interface MonthlySalaryGenerationExitedEmployees_Memory {
  salaryGeneration_Param: MonthlySalaryGenerationExitedEmployees_Param
  dataLock_Param: MonthlySalaryGenerationExitedEmployees_Param
  selected_Tab: string;
}
