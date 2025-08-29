import { OperationResult } from '@utilities/operation-result';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { LoanedMonthlyAttendanceDataMaintenanceDto, LoanedMonthlyAttendanceDataMaintenanceParam, LoanedMonthlyAttendanceDataMaintenanceSource } from '@models/attendance-maintenance/5_1_27_loaned-monthly-attendance-data-maintenance';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_1_27_LoanedMonthlyAttendanceDataMaintenanceService implements IClearCache {
  constructor(private http: HttpClient) { }
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_5_1_27_LoanedMonthlyAttendanceDataMaintenance/`;

  initData = <LoanedMonthlyAttendanceDataMaintenanceSource>{
    param: <LoanedMonthlyAttendanceDataMaintenanceParam>{},
    att_Month_From: null,
    att_Month_To: null,
    isEdit: false,
    isQuery: false,
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    source: <LoanedMonthlyAttendanceDataMaintenanceDto>{},
    data: []
  };
  paramSearch = signal<LoanedMonthlyAttendanceDataMaintenanceSource>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: LoanedMonthlyAttendanceDataMaintenanceSource) => this.paramSearch.set(data)

  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }

  getListDepartment(factory: string,) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params: { factory, language: this.language } });
  }

  getEmployeeID(factory: string) {
    return this.http.get<string[]>(this.apiUrl + "GetEmployeeID", { params: { factory } });
  }

  getEmployeeData(factory: string, att_Month: string, employeeID: string) {
    return this.http.get<OperationResult>(this.apiUrl + "GetEmployeeData", { params: { factory, att_Month, employeeID, language: this.language } });
  }

  getData(pagination: PaginationParam, param: LoanedMonthlyAttendanceDataMaintenanceParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<LoanedMonthlyAttendanceDataMaintenanceDto>>(this.apiUrl + "GetData", { params })
  }

  downloadExcel(param: LoanedMonthlyAttendanceDataMaintenanceParam) {
    return this.http.get<OperationResult>(this.apiUrl + "DownloadExcel", { params: { ...param } });
  }

  add(data: LoanedMonthlyAttendanceDataMaintenanceDto) {
    return this.http.post<OperationResult>(this.apiUrl + "AddNew", data);
  }

  edit(data: LoanedMonthlyAttendanceDataMaintenanceDto) {
    return this.http.put<OperationResult>(this.apiUrl + "Edit", data)
  }
}
