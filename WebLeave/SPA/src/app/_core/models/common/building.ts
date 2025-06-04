export interface Building {
  buildingID: number;
  buildingName: string;
  buildingSym: string;
  buildingCode: string;
  areaID: number | null;
  number: number | null;
  visible: boolean | null;
}