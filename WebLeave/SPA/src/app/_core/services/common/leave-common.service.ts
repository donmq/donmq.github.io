import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { WorkShift } from '@models/common/lunch-break';
import { Holiday } from '@models/leave/personal/holiday';
import { KeyValuePair } from '@utilities/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class LeaveCommonService {
  apiUrl = environment.apiUrl;
  holidays: Holiday[] = [];
  constructor(private http: HttpClient) { }

  getAllCategory(language: string) {
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}LeaveCommon/GetAllCategory`, { params: { language } });
  }

  getListHoliday() {
    return this.http.get<Holiday[]>(`${this.apiUrl}LeaveCommon/GetListHoliday`);
  }

  getCountRestAgent(empId: number, year: number) {
    let params = new HttpParams().set('empId', empId).set('year', year);
    return this.http.get<number>(`${this.apiUrl}LeaveCommon/GetCountRestAgent`, { params });
  }

  checkDateLeave(start: string, end: string, empid: number) {
    let params = new HttpParams().set('start', start).set('end', end).set('empid', empid);
    return this.http.get<any>(`${this.apiUrl}LeaveCommon/CheckDateLeave`, { params });
  }

  checkData() {
    return this.http.get<boolean>(`${this.apiUrl}LeaveCommon/CheckDataDatePicker`);
  }

  getWorkShift(shift: string) {
    return this.http.get<WorkShift>(`${this.apiUrl}LeaveCommon/GetWorkShift`, { params: { shift } });
  }

  getWorkShifts() {
    return this.http.get<WorkShift[]>(`${this.apiUrl}LeaveCommon/GetWorkShifts`);
  }
}
