import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import {
  MonthlySalaryMaintenanceExitedEmployeesSource,
  D_7_25_MonthlySalaryMaintenanceExitedEmployeesSearchParam,
  D_7_25_MonthlySalaryMaintenanceExitedEmployeesMain,
  D_7_25_Query_Sal_Monthly_Detail_Result_Source,
  D_7_25_GetMonthlyAttendanceDataDetailParam,
  D_7_25_MonthlySalaryMaintenance_Update
} from '@models/salary-maintenance/7_1_25-monthly-salary-maintenance-exited-employees';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_25_MonthlySalaryMaintenanceExitedEmployeesService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + 'C_7_1_25_MonthlySalaryMaintenanceExitedEmployees/';

  initData: MonthlySalaryMaintenanceExitedEmployeesSource = <MonthlySalaryMaintenanceExitedEmployeesSource>{
    paramSearch: <D_7_25_MonthlySalaryMaintenanceExitedEmployeesSearchParam>{},
    dataMain: [],
    dataItem: <D_7_25_MonthlySalaryMaintenanceExitedEmployeesMain>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
  }

  paramSource = signal<MonthlySalaryMaintenanceExitedEmployeesSource>(structuredClone(this.initData));
  source = toObservable(this.paramSource);
  setSource = (data: MonthlySalaryMaintenanceExitedEmployeesSource) => this.paramSource.set(data)

  clearParams = () => {
    this.paramSource.set(structuredClone(this.initData))
  }

  constructor(private _http: HttpClient) { }

  getListFactory() {
    const language: string = localStorage.getItem(LocalStorageConstants.LANG)
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListFactory`, { params: { language } });
  }

  getListPermissionGroup(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListPermissionGroup`, { params });
  }

  getListDepartment(factory: string) {
    return this._http.get<KeyValuePair[]>(this.apiUrl + "GetListDepartment", { params: { factory, language: this.language } });
  }

  getListSalaryType() {
    return this._http.get<KeyValuePair[]>(this.apiUrl + 'GetListSalaryType', { params: { language: this.language } });
  }

  search(pagination: PaginationParam, param: D_7_25_MonthlySalaryMaintenanceExitedEmployeesSearchParam) {
    param.lang = this.language
    const params = new HttpParams().appendAll({ ...pagination, ...param });
    return this._http.get<PaginationResult<D_7_25_MonthlySalaryMaintenanceExitedEmployeesMain>>(this.apiUrl + 'GetDataPagination', { params });
  }

  get_MonthlyAttendanceData_MonthlySalaryDetail(param: D_7_25_GetMonthlyAttendanceDataDetailParam) {
    param.language = this.language
    const params = new HttpParams().appendAll({ ...param });
    return this._http.get<D_7_25_Query_Sal_Monthly_Detail_Result_Source>(this.apiUrl + 'Get_MonthlyAttendanceData_MonthlySalaryDetail', { params });
  }

  update(data: D_7_25_MonthlySalaryMaintenance_Update) {
    return this._http.put<OperationResult>(this.apiUrl + "Update", data);
  }

  delete(data: D_7_25_MonthlySalaryMaintenanceExitedEmployeesMain) {
    return this._http.delete<OperationResult>(this.apiUrl + "Delete", { params: {}, body: data });
  }
}
