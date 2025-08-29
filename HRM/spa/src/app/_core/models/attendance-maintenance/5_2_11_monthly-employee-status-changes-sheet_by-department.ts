export interface MonthlyEmployeeStatus_ByDepartmentParam {
  factory: string;
  yearMonth: string;
  firstDate: string;
  lastDate: string;
  level: number;
  levelName: string;
  permissionGroup: string[];
  permissionName: string[];
  lang: string;
}

export interface MonthlyEmployeeStatus_ByDepartmentSource {
  param: MonthlyEmployeeStatus_ByDepartmentParam;
  totalRows: number;
}
