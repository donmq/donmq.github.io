import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { environment } from '@env/environment';
import { inject, Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { toObservable } from '@angular/core/rxjs-interop'

import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import {
  MonthlyExchangeRateSetting_Main,
  MonthlyExchangeRateSetting_Memory,
  MonthlyExchangeRateSetting_Param,
  MonthlyExchangeRateSetting_Update
} from '@models/salary-maintenance/7_1_2_monthly-exchange-rate-setting';
import { ResolveFn } from '@angular/router';
import { OperationResult } from '@utilities/operation-result';
import { Observable } from 'rxjs';
import { IClearCache } from '@services/cache.service';
@Injectable({
  providedIn: 'root'
})
export class S_7_1_2_MonthlyExchangeRateSetting implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_7_1_2_MonthlyExchangeRateSetting/`;
  initData: MonthlyExchangeRateSetting_Memory = <MonthlyExchangeRateSetting_Memory>{
    param: <MonthlyExchangeRateSetting_Param>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <MonthlyExchangeRateSetting_Update>{},
    data: []
  }
  paramSearch = signal<MonthlyExchangeRateSetting_Memory>(structuredClone(this.initData))
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: MonthlyExchangeRateSetting_Memory) => this.paramSearch.set(data)

  constructor(private http: HttpClient) { }

  clearParams() {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getSearchDetail(param: Pagination, filter: MonthlyExchangeRateSetting_Param): Observable<PaginationResult<MonthlyExchangeRateSetting_Main>> {
    filter.lang = this.language
    let params = new HttpParams().appendAll({ ...param, ...filter });
    return this.http.get<PaginationResult<MonthlyExchangeRateSetting_Main>>(`${this.baseUrl}GetSearchDetail`, { params });
  }
  getDropDownList() {
    let param: MonthlyExchangeRateSetting_Param = <MonthlyExchangeRateSetting_Param>{ lang: this.language }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  isExistedData(data: MonthlyExchangeRateSetting_Main) {
    let params = new HttpParams().appendAll({ ...data });
    return this.http.get<OperationResult>(` ${this.baseUrl}IsExistedData`, { params });
  }
  isDuplicatedData(data: MonthlyExchangeRateSetting_Main) {
    let params = new HttpParams().appendAll({ ...data });
    return this.http.get<OperationResult>(` ${this.baseUrl}IsDuplicatedData`, { params });
  }
  putData(param: MonthlyExchangeRateSetting_Param, data: MonthlyExchangeRateSetting_Main[]): Observable<OperationResult> {
    const inputData: MonthlyExchangeRateSetting_Update = <MonthlyExchangeRateSetting_Update>{
      param: param,
      data: data
    }
    return this.http.put<OperationResult>(`${this.baseUrl}PutData`, inputData);
  }
  deleteData(data: MonthlyExchangeRateSetting_Main): Observable<OperationResult> {
    return this.http.delete<OperationResult>(`${this.baseUrl}DeleteData`, { params: {}, body: data });
  }
  postData(param: MonthlyExchangeRateSetting_Param, data: MonthlyExchangeRateSetting_Main[]): Observable<OperationResult> {
    const inputData: MonthlyExchangeRateSetting_Update = <MonthlyExchangeRateSetting_Update>{
      param: param,
      data: data
    }
    return this.http.post<OperationResult>(`${this.baseUrl}PostData`, inputData);
  }
}
export const monthlyExchangeRateSettingResolver: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_7_1_2_MonthlyExchangeRateSetting).getDropDownList();
};
