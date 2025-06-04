import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@env/environment';
import { OperationResult } from '@utilities/operation-result';
import { LeaveDetail } from '@models/common/leave-data';
import { LeaveEditCommentArchive } from '@models/leave/leaveEditCommentArchive';
import { LeaveEditApproval } from '@models/leave/leaveEditApproval';
import { LeaveSendNotiUser } from '@models/leave/leaveSendNotiUser';

@Injectable({ providedIn: 'root' })
export class LeaveDetailService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getLeaveDetail(leaveID: number) {
    var params = new HttpParams().appendAll({ leaveID });
    return this.http.get<LeaveDetail>(this.apiUrl + 'LeaveDetail/Detail', { params });
  }

  editRequestLeave(leaveID: number, reasonAdjust: string) {
    var params = new HttpParams().appendAll({ leaveID, reasonAdjust });
    return this.http.get<OperationResult>(this.apiUrl + 'LeaveDetail/RequestEditLeave', { params });
  }

  editApproval(leaveEditApproval: LeaveEditApproval) {
    return this.http.put<OperationResult>(this.apiUrl + 'LeaveDetail/EditApproval', leaveEditApproval);
  }
  editCommentArchive(leaveEditCommentArchive: LeaveEditCommentArchive) {
    return this.http.put<OperationResult>(this.apiUrl + 'LeaveDetail/EditCommentArchive', leaveEditCommentArchive);
  }
  sendNotitoUser(leaveSendNotiUser: LeaveSendNotiUser) {
    return this.http.put<OperationResult>(this.apiUrl + 'LeaveDetail/SendNotitoUser', leaveSendNotiUser);
  }
}
