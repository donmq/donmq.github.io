export interface EmployeeData extends Employee {
  deptCode: string;
  partNameVN: string;
  partNameEN: string;
  partNameTW: string;
  positionNameVN: string;
  positionNameEN: string;
  positionNameTW: string;
  dateInVN: string;
  dateInEN: string;
  dateInTW: string;
  countRestAgent: number;
  checkUser: boolean;
}

export interface Employee {
  empID: number;
  empName: string;
  empNumber: string;
  dateIn: string | null;
  positionID: number | null;
  descript: string;
  gBID: number | null;
  visible: boolean | null;
  partID: number | null;
  isSun: boolean | null;
}
