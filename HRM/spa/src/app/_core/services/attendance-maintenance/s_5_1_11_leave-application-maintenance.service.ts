import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { toObservable } from '@angular/core/rxjs-interop';
import { BehaviorSubject, Observable } from 'rxjs';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { environment } from '@env/environment';
import {
  LeaveApplicationMaintenance_Main,
  LeaveApplicationMaintenance_MainMemory,
  LeaveApplicationMaintenance_Param,
  LeaveApplicationMaintenance_TypeheadKeyValue
} from '@models/attendance-maintenance/5_1_11_leave-application-maintenance';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root',
})

export class S_5_1_11_Leave_Application_Maintenance implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_5_1_11_LeaveApplicationMaintenance/`;
  initData: LeaveApplicationMaintenance_MainMemory = <LeaveApplicationMaintenance_MainMemory>{
    param: <LeaveApplicationMaintenance_Param>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<LeaveApplicationMaintenance_MainMemory>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: LeaveApplicationMaintenance_MainMemory) => this.paramSearch.set(data)

  paramForm = new BehaviorSubject<LeaveApplicationMaintenance_Main>(null);
  paramForm$ = this.paramForm.asObservable();
  setParamForm = (item: LeaveApplicationMaintenance_Main) => this.paramForm.next(item);

  constructor(private http: HttpClient) { }
  clearParams() {
    this.paramSearch.set(structuredClone(this.initData))
    this.paramForm.next(null)
  }
  isExistedData(data: LeaveApplicationMaintenance_Main) {
    let param: LeaveApplicationMaintenance_Param = <LeaveApplicationMaintenance_Param>{
      factory: data.factory,
      employee_Id: data.employee_Id,
      leave: data.leave_Code,
      leave_Date_From_Str: data.leave_Start
    }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(` ${this.baseUrl}IsExistedData`, { params });
  }
  getDropDownList(factory?: string) {
    let param: LeaveApplicationMaintenance_Param = <LeaveApplicationMaintenance_Param>{
      lang: this.language
    }
    if (factory)
      param.factory = factory
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  getSearchDetail(
    param: Pagination,
    filter: LeaveApplicationMaintenance_Param
  ): Observable<PaginationResult<LeaveApplicationMaintenance_Main>> {
    filter.lang = this.language;
    let params = new HttpParams().appendAll({ ...param, ...filter });
    return this.http.get<PaginationResult<LeaveApplicationMaintenance_Main>>(
      `${this.baseUrl}GetSearchDetail`,
      { params }
    );
  }
  putData(data: LeaveApplicationMaintenance_Main): Observable<OperationResult> {
    return this.http.put<OperationResult>(`${this.baseUrl}PutData`, data);
  }
  postData(data: LeaveApplicationMaintenance_Main): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.baseUrl}PostData`, data);
  }
  deleteData(data: LeaveApplicationMaintenance_Main) {
    return this.http.delete<OperationResult>(`${this.baseUrl}DeleteData`, { body: data });
  }
  downloadExcelTemplate() {
    return this.http.get<OperationResult>(` ${this.baseUrl}DownloadExcelTemplate`);
  }
  uploadExcel(file: FormData) {
    return this.http.post<OperationResult>(`${this.baseUrl}UploadExcel`, file);
  }
  export(params: LeaveApplicationMaintenance_Param) {
    params.lang = this.language
    return this.http.get<OperationResult>(`${this.baseUrl}DownloadExcel`, { params: { ...params } });
  }
}
export const leaveApplicationMaintenanceResolver: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_5_1_11_Leave_Application_Maintenance).getDropDownList();
};
