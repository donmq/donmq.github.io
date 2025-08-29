import { Pagination } from "../../utilities/pagination-utility";
export interface EmployeeAttendanceDataSheetDTO {
    department: string;
    department_Name: string;
    employee_ID: string;
    localFullName: string;
    work_Shift_Type: string;
    attendance: string;
    clock_In: string;
    clock_Out: string;
    overtime_ClockIn: string;
    overtime_ClockOut: string;
    delayHour: number;
    workHour: number;
    normalOvertime: number;
    trainingOvertime: number;
    holidayOvertime: number;
    night: number;
    nightOvertime: number;
    total: number;
    att_Date: string;
}

export interface EmployeeAttendanceDataSheetParam {
    factory: string;
    att_Date_From: string;
    att_Date_To: string;
    work_Shift_Type: string;
    employee_ID: string;
    department: string;
    language: string;
}

export interface EmployeeAttendanceDataSheet_Source {
    currentPage: number,
    param: EmployeeAttendanceDataSheetParam,
    basicCode: EmployeeAttendanceDataSheetDTO
  }

  export interface EmployeeAttendanceDataSheet_Basic {
    param: EmployeeAttendanceDataSheetParam
    data: EmployeeAttendanceDataSheetDTO
  }
