export interface MonthlySalaryGenerationParam {
    factory: string;
    permission_Group: string[];
    year_Month: string;
    employee_ID: string;
    is_Delete: boolean;
    totalRows: number;
}

export interface MonthlyDataLockParam {
    factory: string;
    permission_Group: string[];
    year_Month: string;
    salary_Lock: string;
    totalRows: number;
}
export interface MonthlySalaryGeneration_Memory
{
  salaryGeneration_Param: MonthlySalaryGenerationParam;
  dataLock_Param: MonthlyDataLockParam;
  selectedTab: string;
}
