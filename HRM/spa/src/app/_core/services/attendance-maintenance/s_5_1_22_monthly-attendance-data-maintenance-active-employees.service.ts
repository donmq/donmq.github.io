import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { KeyValuePair } from '@utilities/key-value-pair';
import {
  ActiveEmployeeParam,
  EmpInfo522,
  MaintenanceActiveEmployeesDetail,
  MaintenanceActiveEmployeesDetailParam,
  MaintenanceActiveEmployeesMain,
  MaintenanceActiveEmployeesMemory,
  MaintenanceActiveEmployeesParam
} from '@models/attendance-maintenance/5_1_22_monthly-attendance-data-maintenance-active-employees';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { toObservable } from '@angular/core/rxjs-interop';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root',
})
export class S_5_1_22_MonthlyAttendanceDataMaintenanceActiveEmployeesService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + 'C_5_1_22_MonthlyAttendanceDataMaintenanceActiveEmployees/';
  initData: MaintenanceActiveEmployeesMemory = <MaintenanceActiveEmployeesMemory>{
    param: <MaintenanceActiveEmployeesParam>{ language: localStorage.getItem(LocalStorageConstants.LANG) },
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0,
      totalPage: 0
    },
    data: []
  }
  paramSearch = signal<MaintenanceActiveEmployeesMemory>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: MaintenanceActiveEmployeesMemory) => this.paramSearch.set(data)
  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }
  constructor(private _http: HttpClient) { }

  query(pagination: Pagination, param: MaintenanceActiveEmployeesParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param })
    return this._http.get<PaginationResult<MaintenanceActiveEmployeesMain>>(`${this.apiUrl}Query`, { params });
  }

  add(data: MaintenanceActiveEmployeesDetail) {
    return this._http.post<OperationResult>(`${this.apiUrl}Add`, data);
  }

  edit(data: MaintenanceActiveEmployeesDetail) {
    return this._http.put<OperationResult>(`${this.apiUrl}Edit`, data);
  }

  getDetail(param: MaintenanceActiveEmployeesDetailParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param })
    return this._http.get<MaintenanceActiveEmployeesDetail>(`${this.apiUrl}GetDetail`, { params });
  }

  getEmpInfo(param: ActiveEmployeeParam) {
    param.language = this.language
    return this._http.get<OperationResult>(`${this.apiUrl}GetEmpInfo`, { params: { ...param } });
  }

  getLeaveAllowance(param: MaintenanceActiveEmployeesDetailParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param })
    return this._http.get(`${this.apiUrl}GetLeaveAllowance`, { params });
  }

  download(params: MaintenanceActiveEmployeesParam) {
    params.language = this.language
    return this._http.get<OperationResult>(`${this.apiUrl}Download`, { params: { ...params } });
  }

  getListFactory() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListFactory`, { params: { language: this.language } });
  }

  getListPermissionGroup() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListPermissionGroup`, { params: { language: this.language } });
  }

  getListSalaryType() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListSalaryType`, { params: { language: this.language } });
  }

  getListFactoryAdd() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListFactoryAdd`, { params: { language: this.language } });
  }

  getListDepartment(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}QueryDepartmentList`, { params });
  }
  getEmployeeIDByFactorys(factory: string) {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetEmployeeIDByFactorys`, { params: { factory } });
  }
  getListFactoryByUser() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}getListFactoryByUser`, { params: { language: this.language } });
  }
}
