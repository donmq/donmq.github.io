import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import {
  HRMS_Org_Department,
  HRMS_Org_DepartmentParamSource,
  HRMS_Org_Department_Param,
  Language,
  LanguageParams,
  ListUpperVirtual
} from '@models/organization-management/3_1_1-department-maintenance';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';

@Injectable({
  providedIn: 'root'
})
export class S_3_1_1_DepartmentMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl: string = environment.apiUrl + "C_3_1_1_DepartmentMaintenance/"
  initData: HRMS_Org_DepartmentParamSource = <HRMS_Org_DepartmentParamSource>{
    param: <HRMS_Org_Department_Param>{ status: 'Y' },
    currentPage: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <HRMS_Org_Department>{},
    dataMain: []
  }
  programSource = signal<HRMS_Org_DepartmentParamSource>(structuredClone(this.initData));
  programSource$ = toObservable(this.programSource);
  setSource = (source: HRMS_Org_DepartmentParamSource) => this.programSource.set(source);
  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  }

  constructor(private http: HttpClient) { }

  getData(pagination: PaginationParam, param: HRMS_Org_Department_Param) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<HRMS_Org_Department>>(this.baseUrl + 'GetData', { params });
  }
  add(param: HRMS_Org_Department) {
    return this.http.post<OperationResult>(this.baseUrl + "Add", param);
  }
  edit(param: HRMS_Org_Department) {
    return this.http.put<OperationResult>(this.baseUrl + "Update", param);
  }
  addLanguage(param: Language) {
    return this.http.post<OperationResult>(this.baseUrl + "AddLanguage", param);
  }
  editLanguage(param: Language) {
    return this.http.put<OperationResult>(this.baseUrl + "UpdateLanguage", param);
  }
  getLanguage() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'GetLanguage');
  }
  getDetail(departmentCode: string, division: string, factory: string) {
    let params = new HttpParams().appendAll({ departmentCode, division, factory })
    return this.http.get<LanguageParams[]>(this.baseUrl + 'GetDetail', { params });
  }
  downloadExcel(param: HRMS_Org_Department_Param) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(`${this.baseUrl}DownloadExcel`, { params });
  }
  getListDepartment(division: string, factory: string) {
    let params = new HttpParams().appendAll({ division, factory, lang: this.language })
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'GetListDepartment', { params });
  }
  getListDivision() {
    let params = new HttpParams().appendAll({ lang: this.language })
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'GetListDivision', { params });
  }
  getListFactory(division: string) {
    let params = new HttpParams().appendAll({ division, lang: this.language  })
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'GetListFactory', { params });
  }
  getListLevel() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'GetListLevel', { params: { lang: this.language  } });
  }
  getListUpperVirtual(department_Code: string, division: string, factory: string) {
    let params = new HttpParams().appendAll({ department_Code, division, factory, lang: this.language  })
    return this.http.get<ListUpperVirtual[]>(this.baseUrl + 'GetListUpperVirtual', { params });
  }

  CheckListDeptCode(division: string, factory: string, deptCode: string) {
    let params = new HttpParams().appendAll({ division, factory, deptCode })
    return this.http.get<boolean>(this.baseUrl + 'CheckListDeptCode', { params });
  }
}
