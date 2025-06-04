export interface LeaveDataViewModel extends LeaveData {
  categoryNameVN: string;
  categoryNameEN: string;
  categoryNameTW: string;
  exhibit: boolean | null;
  checked: boolean;
  status: string;
  disable: boolean;
}

export interface LeaveData {
  leaveID: number;
  empID: number | null;
  empNumber: string;
  empName: string;
  deptCode: string;
  cateID: number | null;
  cateSym: string;
  dateLeave: string | null;
  leaveDay: string;
  approved: string;
  timeLine: string;
  comment: string;
  leaveArrange: boolean | null;
  status_Line: boolean | null;
  created: string | null;
  updated: string | null;
  cateNameVN: string;
  cateNameEN: string;
  cateNameTW: string;
  exhibit: boolean | null;
  listComment: string[];
  leaveArchive: string;
  time_Start: string;
  time_End: string;
  time_Applied: string;
  approvedBy: string
  dateIn: string;
  partName: string;
  leavePlus: boolean | null;
  userID: number | null;
  editRequest: number | null;
  status_Lock: boolean | null;
  mailContent_Lock: string;
  commentArchive: string;
  leaveDayReturn: string;
  reasonAdjust: string;
}
