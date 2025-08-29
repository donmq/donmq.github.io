import { OperationResult } from '@utilities/operation-result';
import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@env/environment';
import { KeyValuePair } from '@utilities/key-value-pair';
import {
  ClockInOutTempRecord,
  HRMS_Att_Overtime_TempDto,
  HRMS_Att_Overtime_TempParam,
  OvertimeTemporarySource,
  OvertimeTempPersonal,
} from '@models/attendance-maintenance/5_1_16_overtime_temporary_record_maintenance';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_1_16_OvertimeTemporaryRecordMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + "C_5_1_16_OvertimeTemporaryRecordMaintenance/"

  initData: OvertimeTemporarySource = <OvertimeTemporarySource>{
    isEdit: null,
    source: <HRMS_Att_Overtime_TempDto>{},
    paramQuery: <HRMS_Att_Overtime_TempParam>{},
    dataMain: [],
    pagination: <Pagination>{},
  }

  source = signal<OvertimeTemporarySource>(structuredClone(this.initData));
  source$ = toObservable(this.source);
  setSource = (source: OvertimeTemporarySource) => this.source.set(source);
  clearParams = () => {
    this.source.set(structuredClone(this.initData))
  }

  constructor(private http: HttpClient) { }

  getData(pagination: Pagination, param: HRMS_Att_Overtime_TempParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param })
    return this.http.get<PaginationResult<HRMS_Att_Overtime_TempDto>>(this.apiUrl + 'GetData', { params })
  }

  create(data: HRMS_Att_Overtime_TempDto) {
    return this.http.post<OperationResult>(this.apiUrl + 'Create', data)
  }

  update(data: HRMS_Att_Overtime_TempDto) {
    return this.http.put<OperationResult>(this.apiUrl + 'Update', data)
  }

  delete(data: HRMS_Att_Overtime_TempDto) {
    return this.http.delete<OperationResult>(this.apiUrl + 'Delete', { params: {}, body: data });
  }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { lang: this.language } })
  }
  getListDepartment(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params: { factory, lang: this.language } })
  }
  getListWorkShiftType() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListWorkShiftType', { params: { lang: this.language } })
  }
  getListHoliday() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListHoliday', { params: { lang: this.language } })
  }
  getClockInOutByTempRecord(data: HRMS_Att_Overtime_TempDto) {
    let params = new HttpParams().appendAll({
      factory: data.factory,
      employeeID: data.employee_ID,
      date: data.date.toDateString(),
      lang: this.language
    })
    return this.http.get<ClockInOutTempRecord>(this.apiUrl + 'GetClockInOutByTempRecord', { params });
  }
  getShiftTimeByWorkShift(data: HRMS_Att_Overtime_TempDto) {
    let params = new HttpParams().appendAll({
      factory: data.factory,
      workShiftType: data.work_Shift_Type,
      date: data.date.toDateString(),
      lang: this.language
    })
    return this.http.get<KeyValuePair>(this.apiUrl + 'GetShiftTimeByWorkShift', { params });
  }

  download(param: HRMS_Att_Overtime_TempParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadFileExcel', { params })
  }
}
