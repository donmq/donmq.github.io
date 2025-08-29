import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import {
  IncomeTaxBracketSettingDto,
  IncomeTaxBracketSettingMain,
  IncomeTaxBracketSettingMemory,
  IncomeTaxBracketSettingParam
} from "@models/salary-maintenance/7_1_13_income-tax-bracket-setting";
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_13_IncomeTaxBracketSettingService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_7_1_13_IncomeTaxBracketSetting/`;

  initData: IncomeTaxBracketSettingMemory = <IncomeTaxBracketSettingMemory>{
    param: <IncomeTaxBracketSettingParam>{
      language: localStorage.getItem(LocalStorageConstants.LANG)
    },
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0,
      totalPage: 0
    },
    selectedData: <IncomeTaxBracketSettingDto>{},
    data: []
  }
  paramSearch = signal<IncomeTaxBracketSettingMemory>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: IncomeTaxBracketSettingMemory) => this.paramSearch.set(data)


  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }
  constructor(private _http: HttpClient) { }

  getDataPagination(pagination: Pagination, param: IncomeTaxBracketSettingParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param })
    return this._http.get<PaginationResult<IncomeTaxBracketSettingMain>>(`${this.baseUrl}GetDataPagination`, { params })
  }

  getDetail(dto: IncomeTaxBracketSettingDto) {
    let params = new HttpParams().appendAll({ ...JSON.parse(JSON.stringify(dto)) })
    return this._http.get<IncomeTaxBracketSettingDto>(`${this.baseUrl}GetDetail`, { params })
  }

  create(dto: IncomeTaxBracketSettingDto) {
    return this._http.post<OperationResult>(`${this.baseUrl}Create`, dto);
  }

  update(dto: IncomeTaxBracketSettingDto) {
    return this._http.put<OperationResult>(`${this.baseUrl}Update`, dto);
  }

  delete(dto: IncomeTaxBracketSettingMain) {
    return this._http.delete<OperationResult>(`${this.baseUrl}Delete`, { body: dto });
  }

  getListNationality() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetListNationality`, { params: { language: this.language } });
  }

  getListTaxCode() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetListTaxCode`, { params: { language: this.language } });
  }
  isDuplicatedData(Nation: string, Tax_Code: string, Tax_Level: number, Effective_Month: string) {
    let params = new HttpParams()
      .set('Nation', Nation)
      .set('Tax_Code', Tax_Code)
      .set('Tax_Level', Tax_Level)
      .set('Effective_Month', Effective_Month)
    return this._http.get<OperationResult>(` ${this.baseUrl}IsDuplicatedData`, { params });
  }
}
