export interface LeaveData {
    leaveID: number;
    leaveArchive: string;
    empID: number | null;
    cateID: number | null;
    time_Start: string | null;
    time_End: string | null;
    dateLeave: string | null;
    leaveDay: number | null;
    approved: number | null;
    time_Applied: string | null;
    timeLine: string;
    comment: string;
    leavePlus: boolean | null;
    leaveArrange: boolean | null;
    userID: number | null;
    approvedBy: number | null;
    editRequest: number | null;
    status_Line: boolean | null;
    created: string | null;
    updated: string | null;
    status_Lock: boolean | null;
    mailContent_Lock: string;
    commentArchive: string;
}