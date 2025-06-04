export interface ViewConfirmDaily {
  deptID: number | null;
  deptCode: string;
  deptName: string;
  deptNameVN: string;
  deptNameEN: string;
  deptNameZH: string;
  partID: number | null;
  partNameVN: string;
  partNameEN: string;
  partNameZH: string;
  empName: string;
  empNumber: string;
  cateID: number | null;
  cateSym: string;
  cateNameVN: string;
  cateNameEN: string;
  cateNameZH: string;
  timeStart: string;
  timeEnd: string;
  dateStart: string;
  dateEnd: string;
  leaveDay: string;
  status: string;
  update: string;
  leaveID: number;
}
