import { LeaveData } from "./leave-data.model";

export interface ReportShowModel {
    leaveDate: string;
    dayOfWeek: number;
    seamp: number;
    applied: number;
    approved: number;
    actual: number;
    mpPoolOut: number;
    mpPoolIn: number;
    total: number;
    time_Start: string;
    time_End: string;
    leaveDay: number | null;
    hour: number | null;
    leaveType: string;
    partCode: string;
    deptCode: string;
    employeeNumber: string;
    employeeName: string;
    employeePostition: string;
    leaveStatus: number;
    title: string;
    leaveData: LeaveData;
}