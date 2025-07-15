export interface SearchMachineParams {
  machineId: string;
  pdcId: number | null;
  buildingCode: string;
  buildingId: number;
  cellCode: string;
  category: string;
  sort: string;
  isPaging: boolean;
  positionCode: string;
}
