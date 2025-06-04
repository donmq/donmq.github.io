export interface Part {
    partID: number;
    partName: string;
    partSym: string;
    partCode: string;
    number: number | null;
    deptID: number | null | string;
    visible: boolean | null;
    partNameVN: string;
    partNameEN: string;
    partNameTW: string;
    type: string;
}

export interface PartParam {
  deptID: string,
  partCode: string
  // title excel
  label_PartName: string;
  label_PartCode: string;
  label_Number: string;

}
