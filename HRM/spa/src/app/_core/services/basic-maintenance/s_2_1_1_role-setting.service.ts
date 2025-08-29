import { RoleSettingParam, RoleSettingDetail, RoleSetting_MainMemory, RoleSettingDto } from '@models/basic-maintenance/2_1_1_role-setting';
import { OperationResult } from '@utilities/operation-result';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { KeyValuePair } from '@utilities/key-value-pair';
import { TreeviewItem } from '@ash-mezdo/ngx-treeview';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root',
})

export class S_2_1_1_RoleSetting implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) };
  baseUrl = `${environment.apiUrl}C_2_1_1_RoleSetting/`;
  initData: RoleSetting_MainMemory = <RoleSetting_MainMemory>{
    param: <RoleSettingParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<RoleSetting_MainMemory>(structuredClone(this.initData))
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: RoleSetting_MainMemory) => this.paramSearch.set(data)
  getRadioChecked = (direct: string) => computed(() => this.paramSearch().param?.direct == direct)

  paramForm = new BehaviorSubject<string>(null);
  paramForm$ = this.paramForm.asObservable();
  setParamForm = (item: string) => this.paramForm.next(item);

  constructor(private http: HttpClient) { }

  clearParams() {
    this.paramSearch.set(structuredClone(this.initData))
    this.paramForm.next(null)
  }
  getDropDownList() {
    let params = new HttpParams().set('lang', this.language);
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  getSearchDetail(
    param: Pagination,
    filter: RoleSettingParam
  ): Observable<PaginationResult<RoleSettingDetail>> {
    filter.lang = this.language
    let params = new HttpParams().appendAll({ ...param, ...filter });
    return this.http.get<PaginationResult<RoleSettingDetail>>(
      `${this.baseUrl}GetSearchDetail`,
      { params }
    );
  }
  getProgramGroupDetail(param: RoleSettingParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<TreeviewItem[]>(`${this.baseUrl}GetProgramGroupDetail`, { params });
  }
  getRoleSettingEdit(param: RoleSettingParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<RoleSettingDto>(`${this.baseUrl}GetRoleSettingEdit`, { params });
  }
  getProgramGroupTemplate() {
    let params = new HttpParams().set('lang', this.language);
    return this.http.get<TreeviewItem[]>(`${this.baseUrl}GetProgramGroupTemplate`, { params });
  }
  postRole(data: RoleSettingDto): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.baseUrl}PostRole`, data);
  }
  putRole(data: RoleSettingDto): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.baseUrl}PutRole`, data);
  }
  downloadExcel(param: RoleSettingParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(` ${this.baseUrl}DownloadExcel`, { params });
  }
  deleteRole(role: string, factory: string) {
    let params = new HttpParams().appendAll({ role, factory })
    return this.http.delete<OperationResult>(`${this.baseUrl}DeleteRole`, { params });
  }
  checkRole(role: string) {
    let params = new HttpParams()
      .set('role', role)
    return this.http.get<OperationResult>(` ${this.baseUrl}CheckRole`, { params });
  }
}

export const dataResolver: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_2_1_1_RoleSetting).getDropDownList();
};
