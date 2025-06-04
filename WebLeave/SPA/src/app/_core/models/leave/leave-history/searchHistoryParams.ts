export interface SearchHistoryParams {
    userID: number;
    empId: string;
    status: number;
    categoryId: number;
    startTime: string;
    endTime: string;
    lang: string;
}

export interface HistoryExportParam extends SearchHistoryParams {
    partName: string;
    deptName: string;
    employee: string;
    numberId: string;
    category: string;
    timeStart: string;
    timeEnd: string;
    leaveDay: string;
    status1: string;
    update: string;
}
