import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import {
  AdditionDeductionItemAndAmountSettings_SubMemory,
  AdditionDeductionItemAndAmountSettings_SubData,
  AdditionDeductionItemAndAmountSettings_SubParam,
  AdditionDeductionItemAndAmountSettings_MainData,
  AdditionDeductionItemAndAmountSettings_MainMemory,
  AdditionDeductionItemAndAmountSettings_MainParam
} from "@models/salary-maintenance/7_1_12_addition-deduction-item-and-amount-settings";
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { toObservable } from '@angular/core/rxjs-interop';
import { HttpClient, HttpParams } from '@angular/common/http';
import { OperationResult } from '@utilities/operation-result';
import { KeyValuePair } from '@utilities/key-value-pair';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_12_AdditionDeductionItemAndAmountSettingsService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_7_1_12_AdditionDeductionItemAndAmountSettings/`;

  initData: AdditionDeductionItemAndAmountSettings_MainMemory = <AdditionDeductionItemAndAmountSettings_MainMemory>{
    param: <AdditionDeductionItemAndAmountSettings_MainParam>{
      language: localStorage.getItem(LocalStorageConstants.LANG),
      permission_Group: []
    },
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0,
      totalPage: 0
    },
    data: [],
  }
  paramSearch = signal<AdditionDeductionItemAndAmountSettings_MainMemory>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: AdditionDeductionItemAndAmountSettings_MainMemory) => this.paramSearch.set(data)

  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }
  constructor(private _http: HttpClient) { }

  getDataPagination(pagination: Pagination, param: AdditionDeductionItemAndAmountSettings_MainParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param })
    return this._http.get<PaginationResult<AdditionDeductionItemAndAmountSettings_MainData>>(`${this.baseUrl}GetDataPagination`, { params })
  }

  getDetail(dto: AdditionDeductionItemAndAmountSettings_SubParam) {
    let params = new HttpParams().appendAll({ ...JSON.parse(JSON.stringify(dto)) })
    return this._http.get<AdditionDeductionItemAndAmountSettings_SubMemory>(`${this.baseUrl}GetDetail`, { params })
  }

  checkData(param: AdditionDeductionItemAndAmountSettings_SubParam) {
    let params = new HttpParams().appendAll({ ...JSON.parse(JSON.stringify(param)) })
    return this._http.get<OperationResult>(`${this.baseUrl}CheckData`, { params });
  }

  create(param: AdditionDeductionItemAndAmountSettings_SubParam, data: AdditionDeductionItemAndAmountSettings_SubData[]) {
    const dto = <AdditionDeductionItemAndAmountSettings_SubMemory>{
      data: data,
      param: param
    }
    return this._http.post<OperationResult>(`${this.baseUrl}Create`, dto);
  }

  update(param: AdditionDeductionItemAndAmountSettings_SubParam, data: AdditionDeductionItemAndAmountSettings_SubData[]) {
    const dto = <AdditionDeductionItemAndAmountSettings_SubMemory>{
      data: data,
      param: param
    }
    return this._http.put<OperationResult>(`${this.baseUrl}Update`, dto);
  }

  delete(data: AdditionDeductionItemAndAmountSettings_MainData) {
    return this._http.delete<OperationResult>(`${this.baseUrl}Delete`, {
      body: data,
    });
  }

  getListFactoryByUser() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetListFactoryByUser`, {
      params: { language: this.language },
    });
  }

  getListPermissionGroupByFactory(factory: string,) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetListPermissionGroupByFactory`, { params });
  }

  getListSalaryType() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetListSalaryType`, {
      params: { language: this.language },
    });
  }

  getListAdditionsAndDeductionsType() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetListAdditionsAndDeductionsType`, {
      params: { language: this.language },
    });
  }

  getListAdditionsAndDeductionsItem() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetListAdditionsAndDeductionsItem`, {
      params: { language: this.language },
    });
  }

  download(param: AdditionDeductionItemAndAmountSettings_MainParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this._http.get<OperationResult>(`${this.baseUrl}DownloadFileExcel`, { params })
  }
}
