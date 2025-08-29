import { PaginationResult } from '@utilities/pagination-utility';


export interface FactoryCalendar_MainParam {
  division: string;
  factory: string;
  month: string;
  month_Str: string;
  lang: string;
}
export interface FactoryCalendar_MainData {
  table: PaginationResult<FactoryCalendar_Table>
  calendar: FactoryCalendar_Calendar
}
export interface FactoryCalendar_Table {
  division: string;
  factory: string;
  att_Date: Date;
  att_Date_Str: string;
  type_Code: string;
  type_Code_Name: string;
  describe: string;
  update_By: string;
  update_Time: Date;
  update_Time_Str: string;
}
export interface FactoryCalendar_Calendar {
  weeks: Week[]
}
export interface Week {
  days: Day[]
}
export interface Day {
  date: number
  date_String: string
  day: string
  month: string
  style: string
  division: string;
  factory: string;
}
export interface FactoryCalendar_MainMemory {
  param: FactoryCalendar_MainParam;
  data: FactoryCalendar_MainData;
}
