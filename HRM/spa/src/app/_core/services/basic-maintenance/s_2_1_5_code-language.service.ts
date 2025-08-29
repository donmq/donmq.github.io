import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@env/environment';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop'
import { CodeNameParam, Code_Language, Code_LanguageDetail, Code_LanguageParam, Code_LanguageSource } from '@models/basic-maintenance/2_1_5_code-language';
import { BehaviorSubject } from 'rxjs';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_2_1_5_CodeLanguageService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) };
  apiUrl = `${environment.apiUrl}C_2_1_5_CodeLanguage/`;
  initData: Code_LanguageSource = <Code_LanguageSource>{
    pagination: <Pagination>{ pageNumber: 1, pageSize: 10, totalCount: 0 },
    status: false,
    param: <Code_LanguageParam>{},
    data: []
  }
  codeLanguageSource = signal<Code_LanguageSource>(structuredClone(this.initData));
  codeLanguageSource$ = toObservable(this.codeLanguageSource);
  setSource = (source: Code_LanguageSource) => this.codeLanguageSource.set(source);

  paramSearchSource = new BehaviorSubject<Code_LanguageParam>(null);
  paramSearchSource$ = this.paramSearchSource.asObservable();
  changeParamSearch(paramSearch: Code_LanguageParam) {
    this.paramSearchSource.next(paramSearch);
  }

  constructor(private http: HttpClient) { }

  clearParams() {
    this.codeLanguageSource.set(structuredClone(this.initData))
    this.paramSearchSource.next(null)
  }

  getData(pagination: PaginationParam, param: Code_LanguageParam) {
    param.language_Code = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<Code_Language>>(this.apiUrl + 'GetData', { params });
  }

  deleteData(param: Code_LanguageParam) {
    param.language_Code = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.delete<OperationResult>(this.apiUrl + 'Delete', { params });
  }

  add(model: Code_LanguageDetail) {
    return this.http.post<OperationResult>(this.apiUrl + 'Add', model)
  }

  edit(model: Code_LanguageDetail) {
    return this.http.put<OperationResult>(this.apiUrl + 'Edit', model)
  }

  getDetail(param: Code_LanguageParam) {
    param.language_Code = this.language
    let params = new HttpParams().appendAll({ ...param })
    return this.http.get<Code_LanguageDetail>(this.apiUrl + 'GetDetail', { params })
  }

  getTypeSeq() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetTypeSeq');
  }

  getLanguage() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetLanguage');
  }
  getCode(type_seq: string) {
    let params = new HttpParams().set('type_seq', type_seq);
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetCode', {
      params,
    });
  }

  getCodeName(param: CodeNameParam) {
    param.language_Code = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<string[]>(this.apiUrl + 'GetCodeName', { params });
  }
  exportExcel(param: Code_LanguageParam) {
    param.language_Code = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'Export', { params });
  }
}
