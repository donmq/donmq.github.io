import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@env/environment';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';

import {
  PregnancyMaternityDetail,
  PregnancyMaternityMemory,
  PregnancyMaternityParam,
} from '@models/attendance-maintenance/5_1_5_pregnancy_and_maternity_data';

import { toObservable } from '@angular/core/rxjs-interop';
import { OperationResult } from '@utilities/operation-result';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { IClearCache } from '@services/cache.service';
import { AnyCatcher } from 'rxjs/internal/AnyCatcher';

@Injectable({
  providedIn: 'root'
})
export class S_5_1_5_PregnancyAndMaternityDataMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_5_1_5_PregnancyAndMaternityDataMaintenance`;

  initData: PregnancyMaternityMemory = <PregnancyMaternityMemory>{
    params: <PregnancyMaternityParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0,
      totalPage: 0
    },
    selectedData: <PregnancyMaternityDetail>{},
    datas: []
  }

  paramSearch = signal<PregnancyMaternityMemory>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: PregnancyMaternityMemory) => this.paramSearch.set(data)
  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  constructor(private _http: HttpClient) { }

  add(data: PregnancyMaternityDetail) {
    return this._http.post<OperationResult>(`${this.apiUrl}/Add`, data);
  }

  edit(data: PregnancyMaternityDetail) {
    return this._http.put<OperationResult>(`${this.apiUrl}/Edit`, data);
  }

  delete(data: PregnancyMaternityDetail) {
    return this._http.delete<OperationResult>(`${this.apiUrl}/Delete`, { body: data });
  }

  getListFactory() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/GetListFactory`, { params: { language: this.language } });
  }

  getListDepartment(factory: string,) {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/GetListDepartment`, { params: { factory, language: this.language } });
  }

  getListWorkType(isWorkShiftType: boolean) {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/GetListWorkType`, { params: { language: this.language, isWorkShiftType } });
  }

  query(pagination: Pagination, params: PregnancyMaternityParam) {
    params.language = this.language
    return this._http.get<PaginationResult<PregnancyMaternityDetail>>(`${this.apiUrl}/Query`, { params: { ...pagination, ...params } });
  }

  exportExcel(params: PregnancyMaternityParam) {
    params.language = this.language
    return this._http.get<OperationResult>(`${this.apiUrl}/ExportExcel`, { params: { ...params } });
  }

  getSpecialRegularWorkType(factory: string, work_Type_Before: string) {
    let params = new HttpParams()
      .set('Factory', factory)
      .set('Work_Type_Before', work_Type_Before)
    return this._http.get<any>(`${this.apiUrl}/GetSpecialRegularWorkType`, { params });
  }
}
