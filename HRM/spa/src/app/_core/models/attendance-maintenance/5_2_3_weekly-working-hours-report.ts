export interface WeeklyWorkingHoursReportDto {
    countRecords: number;
}

export interface WeeklyWorkingHoursReportParam {
    factory: string;
    date_Start: string;
    date_End: string;
    level: string;
    department: string;
    kind: string;
    language: string;
}

export interface WorkHoursResult {
    time0: number;
    time1: number;
    time2: number;
}

export interface WeeklyWorkingHoursReport_Source {
    currentPage: number,
    param: WeeklyWorkingHoursReportParam,
    basicCode: WeeklyWorkingHoursReportDto
  }
  
  export interface WeeklyWorkingHoursReport_Basic {
    param: WeeklyWorkingHoursReportParam
    data: WeeklyWorkingHoursReportDto
  }