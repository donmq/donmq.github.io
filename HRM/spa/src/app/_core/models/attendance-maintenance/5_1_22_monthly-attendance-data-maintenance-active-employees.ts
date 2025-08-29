import { Pagination } from '@utilities/pagination-utility';

export interface MaintenanceActiveEmployeesMain {
  division: string;
  factory: string;
  att_Month: string;
  department: string;
  employee_ID: string;
  local_Full_Name: string;
  pass: string;
  resign_Status: string;
  permission_Group: string;
  salary_Type: string;
  salary_Days: string;
  actual_Days: string;
  probation: string;
  isProbation: boolean;
  update_By: string;
  update_Time: string;
}

export interface MaintenanceActiveEmployeesDetail{
  useR_GUID: string;
  factory: string;
  division: string;
  att_Month: string;
  att_Month_Str: string;
  deadline_Start:string;
  deadline_Start_Str:string;
  deadline_End:string;
  deadline_End_Str:string;
  pass:string;
  employee_ID: string;
  local_Full_Name:string;
  department: string;
  department_Name: string;
  department_Code: string;
  salary_Days: string;
  actual_Days: string;
  permission_Group: string;
  salary_Type: string;
  resign_Status: string;
  probation: string;
  isProbation: boolean;
  delay_Early: number;
  no_Swip_Card: number;
  food_Expenses: number;
  night_Eat_Times: number;

  dayShift_Food: number | null; // Số ngày ăn ban ngày
  nightShift_Food: number | null; // Số ngày ăn đêm

  leaves: LeaveAllowance[]
  allowances: LeaveAllowance[]
}



export interface LeaveAllowance{
  code: string;
  codeName: string;
  days: string;
  total: string;
}

export interface MaintenanceActiveEmployeesDetailParam {
  factory: string;
  employee_ID: string;
  att_month: string;
  language: string;
  action?: string;
  department: string;
  isProbation: boolean;
}

export interface MaintenanceActiveEmployeesParam {
  factory: string;
  att_Month_Start: string;
  att_Month_End: string;
  department: string;
  employee_ID: string;
  salary_Days: number | string;
  language: string;
  action?: string;
}

export interface MaintenanceActiveEmployeesMemory {
  param: MaintenanceActiveEmployeesParam;
  pagination: Pagination;
  data: MaintenanceActiveEmployeesMain[];
}

export interface ActiveEmployeeParam {
  factory: string;
  employee_ID: string;
  att_Month: string;
  language: string;
  isAdd: boolean;
  isMonthly: boolean;
}

export interface EmpInfo522 {
  useR_GUID: string;
  local_Full_Name: string;
  department: string;
  division: string;
  permission_Group:string;
}
