import { Pagination } from "@utilities/pagination-utility";

export interface HRMS_Att_Swipecard_SetDto {
  factory: string;
  employee_Start: number | null;
  employee_End: number | null;
  time_Start: number | null;
  time_End: number | null;
  date_Start: number | null;
  date_End: number | null;
  update_By: string;
  update_Time: Date | string;
}

export interface CardSwipingDataFormatSettingMain {
  factory: string;
  employee_Id_Card_No: string;
  date: string;
  time: string;
}

export interface CardSwipingDataFormatSettingSource {
  factory: string
  pagination: Pagination
  data: CardSwipingDataFormatSettingMain[]
}
