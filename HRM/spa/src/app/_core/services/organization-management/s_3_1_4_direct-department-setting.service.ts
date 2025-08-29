import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { toObservable } from '@angular/core/rxjs-interop'
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { OperationResult } from '@utilities/operation-result';
import { KeyValuePair } from '@utilities/key-value-pair';
import {
  Org_Direct_DepartmentParam,
  Org_Direct_DepartmentResult,
  Org_Direct_DepartmentSource
} from '@models/organization-management/3_1_4-direct-department-setting';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_3_1_4_DirectDepartmentSettingService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = `${environment.apiUrl}C_3_1_4_DirectDepartmentSetting/`;

  initData: Org_Direct_DepartmentSource = <Org_Direct_DepartmentSource>{
    param: <Org_Direct_DepartmentParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <Org_Direct_DepartmentResult>{},
    data: []
  }
  paramSearch = signal<Org_Direct_DepartmentSource>(structuredClone(this.initData))
  paramSearch$ = toObservable(this.paramSearch);
  setSearchSource = (source: Org_Direct_DepartmentSource) => this.paramSearch.set(source);

  constructor(private http: HttpClient) { }

  clearParams() {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getData(pagination: Pagination, param: Org_Direct_DepartmentParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<Org_Direct_DepartmentResult>>(this.apiUrl + "getData", { params: params });
  }

  getListDivision() {
    let params = new HttpParams().set('Language',  this.language)
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDivision', { params: params });
  }
  getListFactory(division: string) {
    let params = new HttpParams().set('Division', division).set('Language', this.language)
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: params });
  }
  getListDepartment() {
    let params = new HttpParams().set('Language', this.language)
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params: params });
  }

  getListLine(division: string, factory: string) {
    let params = new HttpParams().set('Division', division).set('Factory', factory)
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListLine', { params: params });
  }
  GetListDirectDepartmentAttribute() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDirectDepartmentAttribute');
  }

  addNew(models: Org_Direct_DepartmentResult[]) {
    return this.http.post<OperationResult>(this.apiUrl + "add", models);
  }
  edit(models: Org_Direct_DepartmentResult[]) {
    return this.http.put<OperationResult>(this.apiUrl + "edit", models);
  }
  delete(models: Org_Direct_DepartmentParam) {
    models.lang = this.language
    return this.http.delete<OperationResult>(this.apiUrl + "delete", { params: {}, body: models });
  }
  getdetail(model: Org_Direct_DepartmentParam) {
    model.lang = this.language
    let params = new HttpParams().appendAll({ ...model })
    return this.http.get<Org_Direct_DepartmentResult[]>(this.apiUrl + 'Getdetail', { params: params });
  }
  downloadExcel(param: Org_Direct_DepartmentParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(` ${this.apiUrl}DownloadExcel`, { params });
  }
  checkDuplicate(models: Org_Direct_DepartmentResult[]) {
    return this.http.put<OperationResult>(this.apiUrl + "CheckDuplicate", models);
  }
}

