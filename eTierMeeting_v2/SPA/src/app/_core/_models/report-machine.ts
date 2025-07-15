export interface ReportMachine {
    totalIdle: number;
    totalInuse: number;
    listReportMachineItem: ReportMachineItem[];
}

export class ReportMachineItem {
    buildingID: string;
    buildingName: string;
    inUse: string;
    idle: string;
}