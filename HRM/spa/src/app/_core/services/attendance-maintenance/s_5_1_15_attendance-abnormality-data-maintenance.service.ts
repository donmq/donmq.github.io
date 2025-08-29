import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import {
  AttendanceAbnormalityDataMaintenanceParam,
  AttendanceAbnormalityDataMaintenanceSource,
  EmployeeData,
  HRMS_Att_Temp_RecordDto
} from '@models/attendance-maintenance/5_1_15_attendance-abnormality-data-maintenance';
import { OperationResult } from '@utilities/operation-result';
import { Observable } from 'rxjs';
import { toObservable } from '@angular/core/rxjs-interop';
import { KeyValuePair } from '@utilities/key-value-pair';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_1_15_AttendanceAbnormalityDataMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_5_1_15_AttendanceAbnormalityDataMaintenance/`;

  constructor(private http: HttpClient) { }

  initData = <AttendanceAbnormalityDataMaintenanceSource>{
    param: <AttendanceAbnormalityDataMaintenanceParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  };
  paramSearch = signal<AttendanceAbnormalityDataMaintenanceSource>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: AttendanceAbnormalityDataMaintenanceSource) => this.paramSearch.set(data)

  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getListFactoryByUser() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactoryByUser', { params: { language: this.language } });
  }

  getListDepartment(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params: { factory, language: this.language } });
  }

  getListWorkShiftType() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListWorkShiftType', { params: { language: this.language } });
  }

  getListAttendance() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListAttendance', { params: { language: this.language } });
  }

  getListUpdateReason() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListUpdateReason', { params: { language: this.language } });
  }

  getListHoliday() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListHoliday', { params: { language: this.language } });
  }

  getData(pagination: PaginationParam, param: AttendanceAbnormalityDataMaintenanceParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<HRMS_Att_Temp_RecordDto>>(this.apiUrl + "GetData", { params })
  }

  addNew(dataList: HRMS_Att_Temp_RecordDto) {
    return this.http.post<OperationResult>(this.apiUrl + "AddNew", dataList);
  }

  edit(data: HRMS_Att_Temp_RecordDto) {
    return this.http.put<OperationResult>(this.apiUrl + "Edit", data)
  }

  delete(data: HRMS_Att_Temp_RecordDto) {
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { params: {}, body: data });
  }

  download(param: AttendanceAbnormalityDataMaintenanceParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadFileExcel', { params })
  }
}
