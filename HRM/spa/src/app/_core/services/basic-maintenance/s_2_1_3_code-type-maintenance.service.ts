import { OperationResult } from '@utilities/operation-result';
import { environment } from '@env/environment';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { BasicCode, HRMS_Basic_Code_TypeDto, HRMS_Basic_Code_TypeParam, HRMS_Type_Code_Source, Language_Dto, ResultMain } from '@models/basic-maintenance/2_1_3_type-code-maintenance';
import { BehaviorSubject } from 'rxjs';
import { toObservable } from '@angular/core/rxjs-interop'
import { KeyValuePair } from '@utilities/key-value-pair';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_2_1_3_CodeTypeMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + "C_2_1_3_TypeCodeMaintenance/";
  initDataBasicCode: BasicCode = <BasicCode>{
    param: <HRMS_Basic_Code_TypeParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<BasicCode>(structuredClone(this.initDataBasicCode));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: BasicCode) => this.paramSearch.set(data)

  initDataCodeSource: HRMS_Type_Code_Source = <HRMS_Type_Code_Source>{}
  typeCodeSource = signal<HRMS_Type_Code_Source>(structuredClone(this.initDataCodeSource));
  typeCodeSource$ = toObservable(this.typeCodeSource);
  setSource = (source: HRMS_Type_Code_Source) => this.typeCodeSource.set(source);

  paramSearchSource = new BehaviorSubject<HRMS_Basic_Code_TypeParam>(null);
  currentParamSearch = this.paramSearchSource.asObservable();

  constructor(private http: HttpClient) { }

  clearParams() {
    this.paramSearch.set(structuredClone(this.initDataBasicCode))
    this.typeCodeSource.set(structuredClone(this.initDataCodeSource))
    this.paramSearchSource.next(null)
  }

  changeParamSearch(paramsearch: HRMS_Basic_Code_TypeParam) {
    this.paramSearchSource.next(paramsearch);
  }

  getAll(pagination: Pagination, param: HRMS_Basic_Code_TypeParam) {
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<ResultMain>>(this.apiUrl + 'getData', { params })
  }

  createLanguage(params: Language_Dto) {
    return this.http.post<OperationResult>(this.apiUrl + 'addNew', params);
  }

  EditLanguageCode(params: Language_Dto) {
    return this.http.put<OperationResult>(this.apiUrl + 'editLanguageCode', params);
  }

  createTypeCode(params: HRMS_Basic_Code_TypeDto) {
    return this.http.post<OperationResult>(this.apiUrl + 'addTypeCode', params);
  }

  update(params: HRMS_Basic_Code_TypeDto) {
    return this.http.put<OperationResult>(this.apiUrl + 'update', params);
  }


  delete(type_Seq: string) {
    return this.http.delete<OperationResult>(this.apiUrl + 'delete', { params: { type_Seq } });
  }

  getLanguage() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetLanguage');
  }

  getDetail(type_Seq: string) {
    let params = new HttpParams().appendAll({ type_Seq })
    return this.http.get<Language_Dto>(this.apiUrl + 'GetDetail', { params });
  }
}

