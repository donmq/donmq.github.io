import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from '@env/environment';
import { Detail } from '@models/leave/detail';
import { LeaveData } from '@models/leave/leaveDataViewModel';
import { OperationResult } from '@utilities/operation-result';
import {
  PaginationParam,
  PaginationResult,
} from '@utilities/pagination-utility';

@Injectable({
  providedIn: 'root',
})
export class LeaveEditDataService {
  baseUrl = environment.apiUrl;
  
  leaveDataSource = new BehaviorSubject<LeaveData>(<LeaveData>{});
  curentLeaveDataSource = this.leaveDataSource.asObservable();

  constructor(private http: HttpClient) { }

  paramLeaveDataEdit(item: LeaveData) {
    this.leaveDataSource.next(item);
  }

  getAllEditLeave(pagination: PaginationParam) {
    let params = new HttpParams().appendAll({ ...pagination });
    return this.http.get<PaginationResult<LeaveData>>(
      this.baseUrl + 'EditLeaveData/GetAllEditLeaveData',
      { params }
    );
  }
  editLeave(LeaveID: number) {
    return this.http.put<OperationResult>(
      `${this.baseUrl}EditLeaveData/EditLeave`,
      LeaveID
    );
  }

  getDetailEmployy(id: number) {
    let leaveID = id == null ? '' : id;
    let params = new HttpParams().appendAll({ leaveID });
    return this.http.get<Detail>(
      this.baseUrl + 'EditLeaveData/GetDetailEmployee',
      { params }
    );
  }
  getDetailById(id: number) {
    let leaveID = id == null ? '' : id;
    let params = new HttpParams().appendAll({ leaveID });
    return this.http.get<LeaveData>(
      this.baseUrl + 'EditLeaveData/GetLeaveByID',
      { params }
    );
  }
}
