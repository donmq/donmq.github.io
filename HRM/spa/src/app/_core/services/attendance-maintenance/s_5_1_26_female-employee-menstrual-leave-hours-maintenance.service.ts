import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import {
  FemaleEmpMenstrualMain,
  FemaleEmpMenstrualMemory,
  FemaleEmpMenstrualParam
} from '@models/attendance-maintenance/5_1_26_female-employee-menstrual-leave-hours-maintenance';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root'
})
export class S_5_1_26_FemaleEmployeeMenstrualLeaveHoursMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_5_1_26_FemaleEmployeeMenstrualLeaveHoursMaintenance`;

  initData = <FemaleEmpMenstrualMemory>{
    params: <FemaleEmpMenstrualParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    datas: []
  };
  paramSearch = signal<FemaleEmpMenstrualMemory>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: FemaleEmpMenstrualMemory) => this.paramSearch.set(data);
  clearParams = () => this.paramSearch.set(structuredClone(this.initData));

  constructor(private _http: HttpClient) { }

  add(data: FemaleEmpMenstrualMain) {
    return this._http.post<OperationResult>(`${this.apiUrl}/Add`, data);
  }

  edit(data: FemaleEmpMenstrualMain) {
    return this._http.put<OperationResult>(`${this.apiUrl}/Edit`, data);
  }

  delete(data: FemaleEmpMenstrualMain) {
    return this._http.delete<OperationResult>(`${this.apiUrl}/Delete`, { body: data });
  }
  downloadExcel(param: FemaleEmpMenstrualParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this._http.get<OperationResult>(`${this.apiUrl}/DownloadExcel`, { params });
  }
  getListFactory() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/GetListFactory`, { params: { language: this.language } });
  }

  getListFactoryAdd() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/GetListFactoryAdd`, { params: { language: this.language } });
  }

  getListDepartment(factory: string) {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/GetListDepartment`, { params: { factory, language: this.language } });
  }

  getDataPagination(pagination: Pagination, params: FemaleEmpMenstrualParam) {
    params.language = this.language
    return this._http.get<PaginationResult<FemaleEmpMenstrualMain>>(`${this.apiUrl}/GetDataPagination`, { params: { ...pagination, ...params } });
  }

  getListFactoryByUser() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/getListFactoryByUser`, { params: { language: this.language } });
  }
}
