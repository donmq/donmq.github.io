import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { LeavePersonal } from '@models/leave/personal/leavePersonal';
import { PersonalDataViewModel } from '@models/leave/personal/personalDataViewModel';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class LeavePersonalService {
  apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  addLeaveDataPersonal(leavePersonal: LeavePersonal) {
    return this.http.post<OperationResult>(`${this.apiUrl}LeavePersonal/AddLeaveDataPersonal`, leavePersonal);
  }

  getData() {
    return this.http.get<PersonalDataViewModel>(`${this.apiUrl}LeavePersonal/GetData`);
  }

  getDataDetail(empNumber: string) {
    var params = new HttpParams().appendAll({ empNumber });
    return this.http.get<PersonalDataViewModel>(`${this.apiUrl}LeavePersonal/GetDataDetail`, { params });
  }

  deleteLeaveDataPerson(leaveId: number, empId: number) {
    let params = new HttpParams().set('leaveId', leaveId).set('empId', empId);
    return this.http.delete<boolean>(`${this.apiUrl}LeavePersonal/DeleteLeaveDataPerson`, { params });
  }
}
