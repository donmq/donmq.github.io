export interface SearchMachineInventory {
    machineID: string;
    machineName: string;
    empName: string;
    plnoName: string;
    placeName: string;
    status: string;
    ownerFty: string;
    supplier: string;
    trDate: string | null;
    statusIventory: number;
    askid: string;
    plno: string;
    cellID: number | null;
    buildingID: number;
    pDCID: number | null;
    isNull: boolean;
}