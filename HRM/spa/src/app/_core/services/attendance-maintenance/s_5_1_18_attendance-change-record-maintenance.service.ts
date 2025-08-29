import { Injectable, signal } from '@angular/core';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { UserForLogged } from '@models/auth/auth';
import {
  AttendanceChangeRecordMaintenanceSource,
  EmployeeInfo,
  HRMS_Att_Change_RecordDto,
  HRMS_Att_Change_Record_Delete_Params,
  HRMS_Att_Change_Record_Params
} from '@models/attendance-maintenance/5_1_18_attendance-change-record-maintenance';
import { KeyValuePair } from '@utilities/key-value-pair';
import { Observable } from 'rxjs';
import { IClearCache } from '@services/cache.service';
@Injectable({
  providedIn: 'root'
})
export class S_5_1_18_AttendanceChangeRecordMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  user: UserForLogged = JSON.parse((localStorage.getItem(LocalStorageConstants.USER)));
  constructor(private http: HttpClient) { }
  apiUrl = `${environment.apiUrl}C_5_1_18_AttendanceChangeRecordMaintenance/`;
  apiCommonUrl = `${environment.apiUrl}Common/`;
  initData = <AttendanceChangeRecordMaintenanceSource>(<AttendanceChangeRecordMaintenanceSource>{
    param: <HRMS_Att_Change_Record_Params>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  });

  paramSearch = signal<AttendanceChangeRecordMaintenanceSource>(structuredClone(this.initData))
  setSource = (data: AttendanceChangeRecordMaintenanceSource) => this.paramSearch.set(data)
  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }
  getData(pagination: PaginationParam, param: HRMS_Att_Change_Record_Params) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<HRMS_Att_Change_RecordDto>>(this.apiUrl + "GetData", { params })
  }

  addNew(param: HRMS_Att_Change_RecordDto) {
    return this.http.post<OperationResult>(`${this.apiUrl}Create`, param, { params: { lang: this.language } });
  }

  edit(param: HRMS_Att_Change_RecordDto) {
    return this.http.put<OperationResult>(this.apiUrl + "Update", param, { params: { lang: this.language } });
  }

  delete(param: HRMS_Att_Change_RecordDto) {
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { params: {}, body: param });
  }

  getListHoliday(typeSeq: string, kind: number, inputChar: string) {
    return this.http.get<KeyValuePair[]>(this.apiCommonUrl + 'GetListBasicCodeListChar1', { params: { language: this.language, typeSeq, kind, inputChar } });
  }

  checkExistedData(data: HRMS_Att_Change_RecordDto) {
    let params = new HttpParams()
      .set('Att_Date', data.att_Date_Str)
      .set('Factory', data.factory)
      .set('Employee_ID', data.employee_ID)
    return this.http.get<OperationResult>(`${this.apiUrl}CheckExistedData`, { params });
  }

  getListFactoryByUser() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactoryByUser', { params: { language: this.language } });
  }
}
