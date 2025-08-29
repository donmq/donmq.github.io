import { DatepickerDateCustomClasses } from "ngx-bootstrap/datepicker";

export interface HRMS_Att_Swipe_Card_Excute_Param {
    factory: string; // nhà máy theo nhân viên quản lý
    clockOffDay: string; // Ngày nghỉ trước đó
    workOnDay: string; // Ngày làm việc hiện tại
    holiday: string; // Ngày nghỉ bình thường (Chủ nhật)
    nationalHoliday: string; // (Ngày lễ quốc khánh)
}

export interface EnabledDateConfig {
    dates: Date[],
    configs: DatepickerDateCustomClasses[],
    nearestDate: Date
}