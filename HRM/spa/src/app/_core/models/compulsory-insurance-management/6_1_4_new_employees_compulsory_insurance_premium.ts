
export interface NewEmployeesCompulsoryInsurancePremium_Param {
  factory: string;
  year_Month: string;
  year_Month_Str: string;
  paid_Salary_Days: string;
  permission_Group: string[];
  lang: string;
  function_Type: string;
  total_Rows: number;
}

export interface NewEmployeesCompulsoryInsurancePremium_Memory {
  generation_Param: NewEmployeesCompulsoryInsurancePremium_Param
  report_Param: NewEmployeesCompulsoryInsurancePremium_Param
  selected_Tab: string;
}
