export interface PreliminaryList {
  roleList: any;
  empName: string;
  empNumber: string;
  visible: boolean | null;
  updateBy: string;
  updateTime: string | null;
  listBuilding: ListBuilding[];
  listCell: ListCell[];
  listHpA15: ListHpa15[];
  //Add new//
  is_Manager: boolean | null;
  is_Preliminary: boolean | null;
  
}

export interface ListBuilding {
  buildingID: number;
}

export interface ListCell {
  cellID: number;
  cellCode: string;
}

export interface ListHpa15 {
  plno: string;
  plnoCode: string;
  buildingID: number;
  cellID: number;
}
