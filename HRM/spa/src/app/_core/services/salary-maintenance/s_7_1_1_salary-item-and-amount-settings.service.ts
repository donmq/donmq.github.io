import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { environment } from '@env/environment';
import { inject, Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { toObservable } from '@angular/core/rxjs-interop'
import {
  SalaryItemAndAmountSettings_MainData,
  SalaryItemAndAmountSettings_MainParam,
  SalaryItemAndAmountSettings_Memory,
  SalaryItemAndAmountSettings_SubParam,
  SalaryItemAndAmountSettings_SubData,
  SalaryItemAndAmountSettings_Update
} from '@models/salary-maintenance/7_1_1_salary-item-and-amount-settings';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { ActivatedRouteSnapshot, ResolveFn } from '@angular/router';
import { Observable } from 'rxjs';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_1_SalaryItemAndAmountSettings implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_7_1_1_SalaryItemAndAmountSettings/`;
  initData: SalaryItemAndAmountSettings_Memory = <SalaryItemAndAmountSettings_Memory>{
    param: <SalaryItemAndAmountSettings_MainParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <SalaryItemAndAmountSettings_Update>{},
    data: []
  }
  paramSearch = signal<SalaryItemAndAmountSettings_Memory>(structuredClone(this.initData))
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: SalaryItemAndAmountSettings_Memory) => this.paramSearch.set(data)

  constructor(private http: HttpClient) { }

  clearParams() {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getSearchDetail(param: Pagination, filter: SalaryItemAndAmountSettings_MainParam): Observable<PaginationResult<SalaryItemAndAmountSettings_MainData>> {
    filter.lang = this.language
    let params = new HttpParams().appendAll({ ...param, ...filter });
    return this.http.get<PaginationResult<SalaryItemAndAmountSettings_MainData>>(`${this.baseUrl}GetSearchDetail`, { params });
  }
  getDropDownList(formType: string) {
    let param: SalaryItemAndAmountSettings_MainParam = <SalaryItemAndAmountSettings_MainParam>{
      lang: this.language,
      formType: formType
    }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  isExistedData(data: SalaryItemAndAmountSettings_MainData) {
    let params = new HttpParams().appendAll({ ...data });
    return this.http.get<OperationResult>(` ${this.baseUrl}IsExistedData`, { params });
  }
  isDuplicatedData(data: SalaryItemAndAmountSettings_SubParam) {
    let params = new HttpParams().appendAll({ ...data });
    return this.http.get<OperationResult>(` ${this.baseUrl}IsDuplicatedData`, { params });
  }
  putData(param: SalaryItemAndAmountSettings_SubParam, data: SalaryItemAndAmountSettings_SubData[]): Observable<OperationResult> {
    const inputData: SalaryItemAndAmountSettings_Update = <SalaryItemAndAmountSettings_Update>{
      param: param,
      data: data
    }
    return this.http.put<OperationResult>(`${this.baseUrl}PutData`, inputData);
  }
  deleteData(data: SalaryItemAndAmountSettings_MainData): Observable<OperationResult> {
    return this.http.delete<OperationResult>(`${this.baseUrl}DeleteData`, { params: {}, body: data });
  }
  postData(param: SalaryItemAndAmountSettings_SubParam, data: SalaryItemAndAmountSettings_SubData[]): Observable<OperationResult> {
    const inputData: SalaryItemAndAmountSettings_Update = <SalaryItemAndAmountSettings_Update>{
      param: param,
      data: data
    }
    return this.http.post<OperationResult>(`${this.baseUrl}PostData`, inputData);
  }
}
export const salaryItemAndAmountSettingsResolver: ResolveFn<KeyValuePair[]> = (route: ActivatedRouteSnapshot) => {
  return inject(S_7_1_1_SalaryItemAndAmountSettings).getDropDownList(route.data.title);
};
