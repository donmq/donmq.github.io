import { HistoryEmp } from "../../common/history-emp";
import { LeaveData } from "../../common/leave-data";

export interface DetailEmployee {
    employee: HistoryEmp;
    listLeave: LeaveData[];
}