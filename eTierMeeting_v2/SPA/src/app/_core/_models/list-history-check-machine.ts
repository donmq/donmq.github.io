export interface HistoryCheckMachine {
    historyCheckMachineID: number;
    totalMachine: number | null;
    totalScans: number | null;
    totalNotScan: number | null;
    totalExist: number | null;
    totalNotExist: number | null;
    userName: string;
    createBy: string;
    typeFile: string;
    createTime: string | null;
}