import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { ResolveFn } from '@angular/router';
import { LangConstants } from '@constants/lang-constants';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import {
  HRMS_Att_Work_Shift,
  HRMS_Att_Work_ShiftParam,
  HRMS_Att_Work_ShiftSource
} from '@models/attendance-maintenance/5_1_2_shift-schedule-setting';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root'
})
export class S_5_1_2_ShiftScheduleSettingService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_5_1_2_ShiftScheduleSetting`;
  initData: HRMS_Att_Work_ShiftSource = <HRMS_Att_Work_ShiftSource>{
    model: null,
    formType: 'add',
    divisions: [],
    factories: [],
    data: [],
    param: <HRMS_Att_Work_ShiftParam>{ language: 'en' },
    pagination: <Pagination>{ pageNumber: 1, pageSize: 10, totalCount: 0 }
  }
  //#region Set data với Signal
  workShiftSource = signal<HRMS_Att_Work_ShiftSource>(structuredClone(this.initData));
  source = toObservable(this.workShiftSource);
  setSource = (source: HRMS_Att_Work_ShiftSource) => this.workShiftSource.set(source);
  clearParams = () => {
    this.workShiftSource.set(structuredClone(this.initData))
  }
  constructor(private _http: HttpClient) { }
  getDivisions() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}/GetDivisions`, { params: { language: this.language } })
  }

  getFactories() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}/GetFactories`, { params: { language: this.language } })
  }

  getFactoriesByDivision(division: string) {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}/GetFactoriesByDivision`, { params: { division, language: this.language } })
  }

  getWorkShiftTypes() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}/GetWorkShiftTypes`, { params: { language: this.language } });
  }

  getDataPagination(pagination: Pagination, filter: HRMS_Att_Work_ShiftParam) {
    filter.language = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...filter });
    return this._http.get<PaginationResult<HRMS_Att_Work_Shift>>(`${this.baseUrl}/GetDataPagination`, { params });
  }

  create(model: HRMS_Att_Work_Shift) {
    return this._http.post<OperationResult>(`${this.baseUrl}/Create`, model);
  }

  update(model: HRMS_Att_Work_Shift) {
    return this._http.put<OperationResult>(`${this.baseUrl}/Update`, model);
  }

}


export const resolverDivisions: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_5_1_2_ShiftScheduleSettingService).getDivisions();
};

export const resolverWorkShiftTypes: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_5_1_2_ShiftScheduleSettingService).getWorkShiftTypes();
};
