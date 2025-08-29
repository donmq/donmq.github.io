
export interface DailyAttendancePostingParam {
  factory: string;
  language: string;
}

export interface DailyAttendancePostingBasic {
  param: DailyAttendancePostingParam
  processedRecords: number
}
