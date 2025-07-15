export interface Building {
  buildingID: number;
  buildingCode: string;
  buildingName: string;
  visible: boolean | null;
  [property: string]: any;
}
