import { Pagination } from "@utilities/pagination-utility";

export interface ResignedEmployeeMain {
  useR_GUID: string;
  pass: string;
  factory: string;
  att_Month: string | Date;
  department: string;
  department_Name: string;
  department_Code: string;
  employee_ID: string;
  local_Full_Name: string;
  resign_Status: string;
  permission_Group: string;
  permission_Group_Name: string;
  salary_Type: string;
  salary_Type_Name: string;
  salary_Days: string;
  actual_Days: string;
  probation: string;
  isProbation: boolean;
  update_By: string;
  update_Time: string | Date | null;
}

export interface ResignedEmployeeDetail extends ResignedEmployeeMain {
  division: string;
  delay_Early: number;
  no_Swip_Card: number;
  food_Expenses: number;
  night_Eat_Times: number;

  
  /**
   * Số ngày ăn trưa
   */
  dayShift_Food: number | null;

  /**
   * Số ngày ăn khuya
   */
  nightShift_Food: number | null;
  
  leaves: LeaveDetailDisplay[];
  allowances: LeaveDetailDisplay[];
  leaveCodes: (number | null)[];
  allowanceCodes: (number | null)[];
}


export interface LeaveDetailDisplay {
  code: string;
  codeName: string;
  days: string;
  total: string;
}

export interface EmpResignedInfo {
  useR_GUID: string;
  local_Full_Name: string;
  department: string;
  division: string;
  permission_Group: string;
}

export interface ResignedEmployeeParam {
  factory: string;
  att_Month_Start: string;
  att_Month_End: string;
  department: string;
  departmentName: string;
  employee_ID: string;
  printBy: string;
  salary_Days: number | null;
  language: string;
  action?: string;
  isProbation: boolean;
  isMonthly: boolean;
}

export interface ResignedEmployeeDetailParam {
  factory: string;
  att_Month: string;
  employee_ID: string;
  uSER_GUID: string;
  language: string;
}

export interface ResignedEmployeeMemory {
  params: ResignedEmployeeParam;
  pagination: Pagination;
  datas: ResignedEmployeeMain[];
}
