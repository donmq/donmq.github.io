import { Injectable, signal } from '@angular/core';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { LeaveSalaryCalculationMaintenance_Basic, LeaveSalaryCalculationMaintenanceDTO, LeaveSalaryCalculationMaintenanceParam } from '@models/salary-maintenance/7_1_3_leave_salary_calculation_maintenance';
import { environment } from '@env/environment';
import { toObservable } from '@angular/core/rxjs-interop';
import { HttpClient, HttpParams } from '@angular/common/http';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_3_Leave_Salary_Calculation_MaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_7_1_3_LeaveSalaryCalculationMaintenance/`;
  //signal
  initData: LeaveSalaryCalculationMaintenance_Basic = <LeaveSalaryCalculationMaintenance_Basic>{
    param: <LeaveSalaryCalculationMaintenanceParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <LeaveSalaryCalculationMaintenanceDTO>{},
    data: [],
  }
  paramSource = signal<LeaveSalaryCalculationMaintenance_Basic>(structuredClone(this.initData));
  source$ = toObservable(this.paramSource);
  setSource = (data: LeaveSalaryCalculationMaintenance_Basic) => this.paramSource.set(data)
  clearParams = () => {
    this.paramSource.set(structuredClone(this.initData))
  }
  constructor(private http: HttpClient) { }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }
  getListLeaveCode() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListLeaveCode', { params: { language: this.language  } });
  }

  getData(pagination: PaginationParam, param: LeaveSalaryCalculationMaintenanceParam) {
    param.language =  this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<LeaveSalaryCalculationMaintenanceDTO>>(this.apiUrl + 'GetData', { params })
  }
  create(data: LeaveSalaryCalculationMaintenanceDTO) {
    return this.http.post<OperationResult>(this.apiUrl + "Create", data);
  }

  update(data: LeaveSalaryCalculationMaintenanceDTO) {
    return this.http.put<OperationResult>(this.apiUrl + "Update", data);
  }

  delete(data: LeaveSalaryCalculationMaintenanceDTO) {
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { body: data });
  }
}
