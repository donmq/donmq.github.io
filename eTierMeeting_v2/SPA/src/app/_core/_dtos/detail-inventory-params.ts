export interface DetailInventory {
  typeInventory: number;
  empNumber: string;
  empName: string;
  countMachine: number | null;
  countMatch: number | null;
  countWrongPosition: number | null;
  countNotScan: number | null;
  percenMatch: string;
  dateStartInventory: string | null;
  dateEndInventory: string | null;
  createTime: string | null;
  isCreateTime: boolean;
}

export interface DetaiHistoryInventory {
  machineID: string;
  machineName: string;
  supplier: string;
  place: string;
  state: string;
  statusSoKiem: number | null;
  statusNameSoKiem: string;
  statusPhucKiem: number | null;
  statusNamePhucKiem: string;
  statusRutKiem: number | null;
  statusNameRutKiem: string;
}

export interface ResultAllInventory {
  listDetail: DetailInventory[];
  listResult: DetaiHistoryInventory[];
}
