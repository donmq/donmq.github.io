export interface HistoryInventory {
    historyInventoryID: number;
    inventoryType: number | null;
    idPlno: string;
    place: string;
    countComplete: number | null;
    countWrongPosition: number | null;
    countNotScan: number | null;
    userName: string;
    empName: string;
    startTimeInventory: string | null;
    endTimeInventory: string | null;
    dateTime: string | null;
    typeFile: string;
}
