import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { HistoryEmp } from '../../models/seahr/employee-managers/HistoryEmp';
import { SeaEmployeeFilter } from '@params/seahr/employee-managers/seaEmployeeFilter';
import { FunctionUtility } from '@utilities/function.utility';
import { KeyValuePair } from '@utilities/key-value-pair';
import { PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SeahrEmployeeManagerService {
  baseApi = `${environment.apiUrl}SeaEmployeeManager`;

  constructor(
    private http: HttpClient,
    private funtionUtility: FunctionUtility,
  ) { }
  dataSource = new BehaviorSubject<SeaEmployeeFilter>(null)
  currentDataSource = this.dataSource.asObservable();
  //#region Main Data
  search(param: PaginationParam, filter: SeaEmployeeFilter) {
    let params = new HttpParams().appendAll({ ...param });
    if (filter.areaId != null)
      params = params.set('areaId', filter.areaId);
    if (filter.departmentId != null)
      params = params.set('departmentId', filter.departmentId);
    if (filter.partId != null)
      params = params.set('partId', filter.partId);

    if (!this.funtionUtility.checkEmpty(filter.employeeId))
      params = params.set('employeeId', filter.employeeId);

    return this.http.get<PaginationResult<HistoryEmp>>(`${this.baseApi}/search`, { params });
  }

  getAreas() {
    return this.http.get<KeyValuePair[]>(`${this.baseApi}/area`);
  }

  getDepartments(areaId: number) {
    let params = new HttpParams().set('areaId', areaId);
    return this.http.get<KeyValuePair[]>(`${this.baseApi}/department`, { params });
  }


  getParts(departmentId: number) {
    let params = new HttpParams().set('deptId', departmentId);
    return this.http.get<KeyValuePair[]>(`${this.baseApi}/part`, { params });
  }
  //#endregion
}
