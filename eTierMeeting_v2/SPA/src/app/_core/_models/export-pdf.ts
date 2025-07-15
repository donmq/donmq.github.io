import { CheckMachineHome } from './check-machine-home';
import { User } from './user';

export interface ResultHistoryCheckMachine {
    listCheckMachine: CheckMachineHome[];
    user: User;
    error: number;
    historyCheckMachine: HistoryCheckMachine;
    totalScans: number;
    totalExist: number;
    totalNotExist: number;
}
export interface HistoryCheckMachine {
    historyCheckMachineID: number;
    totalMachine: number | null;
    totalScans: number | null;
    totalNotScan: number | null;
    totalExist: number | null;
    totalNotExist: number | null;
    createBy: string;
    createTime: string | null;
}