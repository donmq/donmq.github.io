import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import {
  IncomeTaxFreeSetting_MainData,
  IncomeTaxFreeSettingMemory,
  IncomeTaxFreeSetting_MainParam,
  IncomeTaxFreeSetting_SubParam,
  IncomeTaxFreeSetting_Form,
  IncomeTaxFreeSetting_SubData
} from "@models/salary-maintenance/7_1_14_income-tax-free-setting";
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_14_IncomeTaxFreeSettingService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_7_1_14_IncomeTaxFreeSetting/`;

  initData: IncomeTaxFreeSettingMemory = <IncomeTaxFreeSettingMemory>{
    param: <IncomeTaxFreeSetting_MainParam>{
      language: localStorage.getItem(LocalStorageConstants.LANG)
    },
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0,
      totalPage: 0
    },
    data: []
  }
  paramSearch = signal<IncomeTaxFreeSettingMemory>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: IncomeTaxFreeSettingMemory) => this.paramSearch.set(data)

  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }
  constructor(private _http: HttpClient) { }

  getDataPagination(pagination: Pagination, param: IncomeTaxFreeSetting_MainParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param })
    return this._http.get<PaginationResult<IncomeTaxFreeSetting_MainData>>(`${this.baseUrl}GetDataPagination`, { params })
  }

  getDetail(dto: IncomeTaxFreeSetting_SubParam) {
    let params = new HttpParams().appendAll({ ...JSON.parse(JSON.stringify(dto)) })
    return this._http.get<IncomeTaxFreeSetting_SubData[]>(`${this.baseUrl}GetDetail`, { params })
  }

  isDuplicatedData(data: IncomeTaxFreeSetting_SubParam) {
    let params = new HttpParams().appendAll({ ...data });
    return this._http.get<OperationResult>(` ${this.baseUrl}IsDuplicatedData`, { params });
  }

  create(param: IncomeTaxFreeSetting_SubParam, data: IncomeTaxFreeSetting_SubData[]) {
    const dto = <IncomeTaxFreeSetting_Form>{
      data: data,
      param: param
    }
    return this._http.post<OperationResult>(`${this.baseUrl}Create`, dto);
  }

  update(param: IncomeTaxFreeSetting_SubParam, data: IncomeTaxFreeSetting_SubData[]) {
    const dto = <IncomeTaxFreeSetting_Form>{
      data: data,
      param: param
    }
    return this._http.put<OperationResult>(`${this.baseUrl}Update`, dto);
  }

  delete(dto: IncomeTaxFreeSetting_MainData) {
    return this._http.delete<OperationResult>(`${this.baseUrl}Delete`, { body: dto });
  }

  getListFactoryByUser() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetListFactoryByUser`, { params: { language: this.language } });
  }

  getListFactory() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetListFactory`, { params: { language: this.language } });
  }

  getListType() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetListType`, { params: { language: this.language } });
  }

  getListSalaryType() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetListSalaryType`, { params: { language: this.language } });
  }

}
