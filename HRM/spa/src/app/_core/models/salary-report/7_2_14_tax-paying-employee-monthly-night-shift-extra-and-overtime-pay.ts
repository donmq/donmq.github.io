export interface NightShiftExtraAndOvertimePayParam {
  factory: string;
  year_Month: string;
  permission_Group: string[];
  department: string;
  employeeID: string;
  userName: string;
  language: string;
}

export interface NightShiftExtraAndOvertimePaySource {
  param: NightShiftExtraAndOvertimePayParam;
  totalRows: number;
}
