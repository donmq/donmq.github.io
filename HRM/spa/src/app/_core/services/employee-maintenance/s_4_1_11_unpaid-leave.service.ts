import { AddAndEditParam, HRMS_Emp_Unpaid_LeaveDto, UnpaidLeaveParam, UnpaidLeaveSource } from '@models/employee-maintenance/4_1_11_unpaid-leave';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { OperationResult } from '@utilities/operation-result';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class S_4_1_11_UnpaidLeaveService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  constructor(private http: HttpClient) { }
  apiUrl = `${environment.apiUrl}C_4_1_11_UnpaidLeave/`;
  initData: UnpaidLeaveSource = <UnpaidLeaveSource>{
    param: <UnpaidLeaveParam>{},
    onboardDate: null,
    leaveStartFrom: null,
    leaveStartTo: null,
    leaveEndFrom: null,
    leaveEndTo: null,
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<UnpaidLeaveSource>(JSON.parse(JSON.stringify(this.initData)));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: UnpaidLeaveSource) => this.paramSearch.set(data)
  clearParams = () => {
    this.paramSearch.set(JSON.parse(JSON.stringify(this.initData)))
  }

  getData(pagination: PaginationParam, param: UnpaidLeaveParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<HRMS_Emp_Unpaid_LeaveDto>>(this.apiUrl + "GetData", { params })
  }

  downloadExcel(param: UnpaidLeaveParam) {
    param.lang = this.language
    return this.http.get<OperationResult>(
      this.apiUrl + "DownloadExcel", { params: { ...param } }
    );
  }

  addNew(param: AddAndEditParam) {
    return this.http.post<OperationResult>(`${this.apiUrl}AddNew`, param);
  }

  edit(param: AddAndEditParam) {
    return this.http.put<OperationResult>(this.apiUrl + "Edit", param)
  }

  delete(model: HRMS_Emp_Unpaid_LeaveDto) {
    return this.http.delete<OperationResult>(
      this.apiUrl + "Delete", { params: {}, body: model }
    );
  }

  getListDivision() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListDivision', { params: { language: this.language } }
    );
  }

  getListFactory(division: string) {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListFactory', { params: { division, language: this.language } }
    );
  }

  getListDepartment(division: string, factory: string) {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListDepartment', { params: { division, factory, language: this.language } }
    );
  }

  getListLeaveReason() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListLeaveReason', { params: { language: this.language } }
    );
  }

  getEmployeeID() {
    return this.http.get<string[]>(this.apiUrl + "GetEmployeeID", {});
  }

  getEmployeeData(factory: string, employeeID: string): Observable<HRMS_Emp_Unpaid_LeaveDto[]> {
    return this.http.get<HRMS_Emp_Unpaid_LeaveDto[]>(
      this.apiUrl + "GetEmployeeData", { params: { factory, employeeID, language: this.language } }
    );
  }

  getSeq(param: AddAndEditParam) {
    const params = new HttpParams()
      .set('division', param.division)
      .set('factory', param.factory)
      .set('employeeID', param.employee_ID)
    return this.http.get<OperationResult>(this.apiUrl + 'GetSeq', { params });
  }
}
