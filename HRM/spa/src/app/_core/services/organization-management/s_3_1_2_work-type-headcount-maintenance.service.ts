import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject, signal, } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { ResolveFn } from '@angular/router';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import {
  DepartmentNameObject,
  HRMS_Org_Work_Type_Headcount,
  HRMS_Org_Work_Type_HeadcountDataMain,
  HRMS_Org_Work_Type_HeadcountParam,
  HRMS_Org_Work_Type_HeadcountSource,
  HRMS_Org_Work_Type_HeadcountUpdate
} from '@models/organization-management/3_1_2_work-type-headcount-maintenance';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination } from '@utilities/pagination-utility';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class S_3_1_2_WorktypeHeadcountMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_3_1_2_WorkTypeHeadCountMaintenance`;

  //#region Set data với Signal
  initData: HRMS_Org_Work_Type_HeadcountSource = <HRMS_Org_Work_Type_HeadcountSource>{
    param: <HRMS_Org_Work_Type_HeadcountParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    model: null,
    data: [],
    totalActualNumber: 0,
    totalHeadCount: 0
  }
  workTypeHeadcountSource = signal<HRMS_Org_Work_Type_HeadcountSource>(structuredClone(this.initData));
  workTypeHeadcountSource$ = toObservable(this.workTypeHeadcountSource);
  setSource = (source: HRMS_Org_Work_Type_HeadcountSource) => this.workTypeHeadcountSource.set(source);
  clearParams = () => {
    this.workTypeHeadcountSource.set(structuredClone(this.initData))
  }
  //#endregion

  constructor(private _http: HttpClient) { }

  getDivisions() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}/GetDivisions`, { params: { language: this.language } });
  }

  getFactories() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}/GetFactories`, { params: { language: this.language  } });
  }

  getFactoriesByDivision(division: string) {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}/GetFactoriesByDivision`, { params: { division, language: this.language  } });
  }

  getDepartments() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}/GetDepartments`, { params: { language: this.language  } });
  }

  getDepartmentsByDivisionFactory(division: string, factory: string) {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}/GetDepartmentsByDivisionFactory`, { params: { division, factory, language: this.language  } });
  }

  getDepartmentName(filter: HRMS_Org_Work_Type_HeadcountParam) {
    filter.language = this.language
    let params = new HttpParams().appendAll({ ...filter });
    return this._http.get<DepartmentNameObject>(`${this.baseUrl}/GetDepartmentName`, { params });
  }

  getWorkCodeNames() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}/GetWorkCodeNames`);
  }

  getListUpdate(filter: HRMS_Org_Work_Type_HeadcountParam) {
    filter.language = this.language
    let params = new HttpParams().appendAll({ ...filter });
    return this._http.get<HRMS_Org_Work_Type_Headcount[]>(`${this.baseUrl}/GetListUpdate`, { params });
  }



  getDataMainPagination(param: Pagination, filter: HRMS_Org_Work_Type_HeadcountParam): Observable<HRMS_Org_Work_Type_HeadcountDataMain> {
    filter.language = this.language
    let params = new HttpParams().appendAll({ ...param, ...filter });
    return this._http.get<HRMS_Org_Work_Type_HeadcountDataMain>(`${this.baseUrl}/GetDataPagination`, { params });
  }

  create(model: HRMS_Org_Work_Type_Headcount[]) {
    return this._http.post<OperationResult>(`${this.baseUrl}/Create`, model);
  }

  update(model: HRMS_Org_Work_Type_HeadcountUpdate) {
    return this._http.put<OperationResult>(`${this.baseUrl}/Update`, model);
  }


  delete(model: HRMS_Org_Work_Type_Headcount) {
    return this._http.delete<OperationResult>(`${this.baseUrl}/Delete`, { params: {}, body: model });
  }
  downloadExcel(param: HRMS_Org_Work_Type_HeadcountParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this._http.get<OperationResult>(`${this.baseUrl}/DownloadExcel`, { params });
  }
}

export const resolverDivisions: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_3_1_2_WorktypeHeadcountMaintenanceService).getDivisions();
};
export const resolverFactories: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_3_1_2_WorktypeHeadcountMaintenanceService).getFactories();
};

