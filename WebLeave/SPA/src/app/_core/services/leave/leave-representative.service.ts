import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { LeaveDataViewModel } from '@models/leave/leaveDataViewModel';
import { LeavePersonal } from '@models/leave/personal/leavePersonal';
import { RepresentativeDataViewModel } from '@models/leave/representative/representativeDataViewModel';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class LeaveRepresentativeService {
  apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  addLeaveData(leavePersonal: LeavePersonal) {
    return this.http.post<boolean>(`${this.apiUrl}LeaveRepresentative/AddLeaveData`, leavePersonal);
  }

  getDataLeave() {
    return this.http.get<RepresentativeDataViewModel[]>(`${this.apiUrl}LeaveRepresentative/GetDataLeave`);
  }

  getEmployeeInfo(empNumber: string) {
    return this.http.get<OperationResult>(`${this.apiUrl}LeaveRepresentative/GetEmployeeInfo`, { params: { empNumber }});
  }

  getListOnTime(leaveId: number) {
    return this.http.get<LeaveDataViewModel[]>(`${this.apiUrl}LeaveRepresentative/GetListOnTime`, { params: { leaveId }});
  }

  deleteLeave(leaveDatas: RepresentativeDataViewModel[]) {
    return this.http.delete<boolean>(`${this.apiUrl}LeaveRepresentative/DeleteLeave`, { body: leaveDatas });
  }
}
