import { toObservable } from '@angular/core/rxjs-interop';
import {
  MonthlyWorkingHoursLeaveHoursReportParam,
  MonthlyWorkingHoursLeaveHoursReportSource
} from '@models/attendance-maintenance/5_2_17_monthlyworkinghours-leavehoursreport';
import { OperationResult } from '@utilities/operation-result';
import { environment } from '@env/environment';
import { Injectable, signal } from '@angular/core';
import { KeyValuePair } from '@utilities/key-value-pair';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_2_17_MonthlyWorkingHoursLeaveHoursReportService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_5_2_17_MonthlyWorkingHoursLeaveHoursReport`;

  initData: MonthlyWorkingHoursLeaveHoursReportSource = <MonthlyWorkingHoursLeaveHoursReportSource>{
    param: <MonthlyWorkingHoursLeaveHoursReportParam>{
      option: 'WorkingHours',
      permissionGroup: [],
    },
    yearMonth_Value: null,
    totalRows: 0
  }
  programSource = signal<MonthlyWorkingHoursLeaveHoursReportSource>(structuredClone(this.initData));
  programSource$ = toObservable(this.programSource);

  setSource = (source: MonthlyWorkingHoursLeaveHoursReportSource) => this.programSource.set(source);
  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  }

  constructor(private _http: HttpClient) { }

  getTotalRows(param: MonthlyWorkingHoursLeaveHoursReportParam) {
    param.language = this.language;
    let params = new HttpParams().appendAll({ ...param });
    return this._http.get<number>(`${this.apiUrl}/GetTotalRows`, { params })
  }

  getListFactory() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/GetListFactory`, { params: { language: this.language } });
  }

  getListPermissionGroup(factory: string) {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/GetListPermissionGroup`, { params: { factory, language: this.language } });
  }

  exportExcel(params: MonthlyWorkingHoursLeaveHoursReportParam) {
    params.language = this.language;
    return this._http.get<OperationResult>(`${this.apiUrl}/DownloadExcel`, { params: { ...params } });
  }
}
