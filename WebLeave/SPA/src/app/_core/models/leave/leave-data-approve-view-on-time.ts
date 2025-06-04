import { LeaveDataApprove } from "./leave-data-approve";

export interface LeaveDataApproveViewOnTime {
    listLeaveDataApprove: LeaveDataApprove[];
    userViewOnTime: string;
    categoryViewOnTime: string;
    leaveTimeViewOnTime: string;
    timeViewOnTime: string;
    timeStartViewOnTime: string;
    timeEndViewOnTime: string;
    leaveIDViewOnTime: number;
}