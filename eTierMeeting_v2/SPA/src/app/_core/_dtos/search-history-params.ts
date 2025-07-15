export interface SearchHistoryParams {
    machineId: string;
    pdcId: number | null;
    buildingCode: number | null;
    cellName: number | null;
    positionCode: string;
    timeStart: string;
    timeEnd: string;
    sort: string;
    isPaging: boolean;
}
