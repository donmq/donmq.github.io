import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { OperationResult } from '@utilities/operation-result';
import { DetailEmployee } from '@models/seahr/edit-leave/detail-employee';
import { LeaveData } from '@models/common/leave-data';
@Injectable({
  providedIn: 'root'
})
export class SeaEditLeaveService {
  baseUrl = environment.apiUrl
  constructor(private http: HttpClient) { }

  getAllEditLeave(pagination: PaginationParam) {
    let params = new HttpParams().appendAll({ ...pagination });
    return this.http.get<PaginationResult<LeaveData>>(`${this.baseUrl}EditLeave/GetAllEditLeave`, { params });
  }

  acceptEditLeave(LeaveID: number) {
    return this.http.put<OperationResult>(`${this.baseUrl}EditLeave/AcceptEditLeave`, LeaveID);
  }

  getDetailEmployee(EmployeeID: number) {
    let params = new HttpParams().appendAll({ EmployeeID });
    return this.http.get<DetailEmployee>(`${this.baseUrl}EditLeave/GetDetailEmployee`, { params });
  }

  rejectEditLeave(LeaveID: number) {
    return this.http.put<OperationResult>(`${this.baseUrl}EditLeave/RejectEditLeave`, LeaveID);
  }
}
