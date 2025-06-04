import { LunchBreak } from "./lunch-break";

export interface LeaveData {
  leaveID: number;
  leaveArchive: string;
  empID: number | null;
  cateID: number | null;
  time_Start: string | null;
  time_End: string | null;
  dateLeave: string | null;
  leaveDay: number | null;
  approved: number | null;
  time_Applied: string | null;
  timeLine: string;
  comment: string;
  leavePlus: boolean | null;
  leaveArrange: boolean | null;
  userID: number | null;
  approvedBy: number | null;
  editRequest: number | null;
  status_Line: boolean | null;
  created: string | null;
  updated: string | null;
  status_Lock: boolean | null;
  mailContent_Lock: string;
  commentArchive: string;
  reasonAdjust: string;

  empName: string | null;
  empNumber: string | null;
  partCode: string | null;
  partCodeTruncate: string | null;
  cateSym: string | null;
  cateNameVN: string | null;
  cateNameEN: string | null;
  cateNameZH: string | null;
  cateName_vi: string | null;
  cateName_en: string | null;
  cateName_zh: string | null;
  leaveDayByString: string | null;
  checkbox: boolean | null;
  exhibit: boolean | null;
  partID: number | null;
  partNameVN: string | null;
  partNameEN: string | null;
  partNameZH: string | null;
  partName_vi: string | null;
  partName_en: string | null;
  partName_zh: string | null;
  deptNameVN: string | null;
  deptNameEN: string | null;
  deptNameZH: string | null;
  deptName_vi: string | null;
  deptName_en: string | null;
  deptName_zh: string | null;
  dateIn: string | null;
  commentLeave: number | null;
  deptCode: string;
  approvedString: string;
  sender: string | null;
  sendBy: string | null;
  translatedComment: string | null;

  numberDay: string;
  category: string;
  statusString: string;
  fullName: string;
  gBID: number | null;
  partSym: string;
  deptSym: string;
  buildingSym: string;
  areaSym: string;
  approveName: string;
  roleEditComments: number;
  employeeId: string;
  commentList: string[];
  departmentName: string;
  approveList: string[];
  lunchBreakVN: string;
  lunchBreakEN: string;
  lunchBreakZH: string;
}

export interface LeaveDetail {
  editCommentArchive: boolean;
  notiUser: boolean;
  roleApproved: boolean;
  enablePreviousMonthEditRequest: boolean;
  leaveData: LeaveData;
  approvalPersons: string[];
  deletedLeaves: LeaveDeleteDetail[];
}

export interface LeaveDeleteDetail {
  timeLine: string;
  comment: string;
  cateNameVN: string;
  cateNameEN: string;
  cateNameZH: string;
  translatedComment: string;
}
