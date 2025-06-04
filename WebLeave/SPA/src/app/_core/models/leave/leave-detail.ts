import { LeaveData } from "../common/leave-data";

export interface LeaveDetail {
  editCommentArchive: boolean;
  notiUser: boolean;
  roleApproved: boolean;
  enablePreviousMonthEditRequest: boolean;
  leaveData: LeaveData;
  approvalPersons: string[];
  deletedLeaves: LeaveData[];
}