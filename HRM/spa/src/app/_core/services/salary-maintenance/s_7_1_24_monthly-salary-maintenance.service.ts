import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { MonthlySalaryMaintenance_Basic, MonthlySalaryMaintenance_Delete, MonthlySalaryMaintenance_Personal, MonthlySalaryMaintenance_Update, MonthlySalaryMaintenanceDetail, MonthlySalaryMaintenanceDto, MonthlySalaryMaintenanceParam } from '@models/salary-maintenance/7_1_24_monthly-salary-maintenance';
import { IClearCache } from '@services/cache.service';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { KeyValuePair } from '@utilities/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_24_MonthlySalaryMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_7_1_24_MonthlySalaryMaintenance/`;
  //signal
  initData: MonthlySalaryMaintenance_Basic = <MonthlySalaryMaintenance_Basic>{
    param: <MonthlySalaryMaintenanceParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <MonthlySalaryMaintenanceDto>{},
    data: []
  }
  paramSource = signal<MonthlySalaryMaintenance_Basic>(structuredClone(this.initData));
  source$ = toObservable(this.paramSource);
  setSource = (data: MonthlySalaryMaintenance_Basic) => this.paramSource.set(data)
  clearParams = () => {
    this.paramSource.set(structuredClone(this.initData))
  }
  constructor(private http: HttpClient) { }
  getListFactory() {
    const language: string = localStorage.getItem(LocalStorageConstants.LANG)
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language } });
  }
  getListDepartment(factory: string) {
    const language: string = localStorage.getItem(LocalStorageConstants.LANG)
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params: { language, factory } });
  }
  getListSalaryType() {
    const language: string = localStorage.getItem(LocalStorageConstants.LANG)
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListSalaryType', { params: { language } });
  }
  getListPermissionGroup(factory: string) {
    const language: string = localStorage.getItem(LocalStorageConstants.LANG)
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListPermissionGroup', { params: { factory, language } });
  }
  getData(pagination: PaginationParam, param: MonthlySalaryMaintenanceParam) {
    param.language = localStorage.getItem(LocalStorageConstants.LANG);
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<MonthlySalaryMaintenanceDto>>(this.apiUrl + 'GetData', { params })
  }
  getListTypeHeadEmployeeID(factory: string) {
    return this.http.get<string[]>(this.apiUrl + 'GetListTypeHeadEmployeeID', { params: { factory } });
  }
  getDetailPersonal(factory: string, employee_ID: string) {
    return this.http.get<MonthlySalaryMaintenance_Personal>(this.apiUrl + 'GetDetailPersonal', { params: { factory, employee_ID } })
  }
  getDetail(data: MonthlySalaryMaintenanceDto) {
    data.language = localStorage.getItem(LocalStorageConstants.LANG)
    let params = new HttpParams().appendAll({ ...data });
    return this.http.get<MonthlySalaryMaintenanceDetail>(this.apiUrl + 'GetDetail', { params })
  }
  update(data: MonthlySalaryMaintenance_Update) {
    return this.http.put<OperationResult>(this.apiUrl + "Update", data);
  }
  delete(data: MonthlySalaryMaintenance_Delete) {
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { body: data });
  }
}
