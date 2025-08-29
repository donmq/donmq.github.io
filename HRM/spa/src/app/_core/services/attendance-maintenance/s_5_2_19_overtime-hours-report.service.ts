import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import {
  OvertimeHoursReport,
  OvertimeHoursReportMemory,
  OvertimeHoursReportParam
} from '@models/attendance-maintenance/5_2_19_overtime-hours-report';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root'
})
export class S_5_2_19_OvertimeHoursReportService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_5_2_19_OvertimeHoursReport`;

  initData = <OvertimeHoursReportMemory>{
    params: <OvertimeHoursReportParam>{
      factory: '',
      department: '',
      work_Shift_Type: '',
      kind: "O",
      lang: localStorage.getItem(LocalStorageConstants.LANG)
    },
    datas: []
  };
  paramSearch = signal<OvertimeHoursReportMemory>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: OvertimeHoursReportMemory) => this.paramSearch.set(data);
  clearParams = () => this.paramSearch.set(structuredClone(this.initData));

  constructor(private _http: HttpClient) { }

  getListFactoryAdd() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/GetListFactoryAdd`, { params: { language: this.language } });
  }

  getListDepartment(factory: string) {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/GetListDepartment`, { params: { factory, language: this.language } });
  }
  getListWorkShiftType() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/GetListWorkShiftType`, { params: { language: this.language } });
  }
  GetListPermissionGroup(factory: string) {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/GetListPermissionGroup`, { params: { language: this.language, factory } });
  }
  getData(params: OvertimeHoursReportParam) {
    params.lang = this.language
    return this._http.get<OvertimeHoursReport[]>(`${this.apiUrl}/GetData`, { params: { ...params } });
  }
  exportExcel(params: OvertimeHoursReportParam) {
    params.lang = this.language
    return this._http.get<OperationResult>(`${this.apiUrl}/Export`, { params: { ...params } });
  }
}
