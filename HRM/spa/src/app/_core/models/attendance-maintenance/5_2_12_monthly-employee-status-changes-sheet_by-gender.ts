export interface MonthlyEmployeeStatus_ByGenderParam {
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

export interface MonthlyEmployeeStatus_ByGenderSource {
  param: MonthlyEmployeeStatus_ByGenderParam;
  totalRows: number;
}
