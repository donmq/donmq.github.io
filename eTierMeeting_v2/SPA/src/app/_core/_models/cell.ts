export interface Cell {
  cellID: number;
  cellCode: string;
  cellName: string;
  pdcid: number | null;
  pdcName: string;
  buildingID: number | null;
  buildingName: string;
  buildingCode: string;
  updateBy: string;
  visible: boolean | null;
  updateTime: string | null;
  [property: string]: any;
}
