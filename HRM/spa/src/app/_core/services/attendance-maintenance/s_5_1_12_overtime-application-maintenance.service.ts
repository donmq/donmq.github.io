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
  OvertimeApplicationMaintenance_Main,
  OvertimeApplicationMaintenance_MainMemory,
  OvertimeApplicationMaintenance_Param,
  OvertimeApplicationMaintenance_TypeheadKeyValue
} from '@models/attendance-maintenance/5_1_12_overtime-application-maintenance';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root',
})

export class S_5_1_12_Overtime_Application_Maintenance implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_5_1_12_OvertimeApplicationMaintenance/`;
  initData: OvertimeApplicationMaintenance_MainMemory = <OvertimeApplicationMaintenance_MainMemory>{
    param: <OvertimeApplicationMaintenance_Param>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<OvertimeApplicationMaintenance_MainMemory>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: OvertimeApplicationMaintenance_MainMemory) => this.paramSearch.set(data)

  paramForm = new BehaviorSubject<OvertimeApplicationMaintenance_Main>(null);
  paramForm$ = this.paramForm.asObservable();
  setParamForm = (item: OvertimeApplicationMaintenance_Main) => this.paramForm.next(item);

  constructor(private http: HttpClient) { }
  clearParams() {
    this.paramSearch.set(structuredClone(this.initData))
    this.paramForm.next(null)
  }
  isExistedData(data: OvertimeApplicationMaintenance_Main) {
    let param: OvertimeApplicationMaintenance_Param = <OvertimeApplicationMaintenance_Param>{
      factory: data.factory,
      employee_Id: data.employee_Id,
      overtime_Date_Str: data.overtime_Date_Str
    }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(` ${this.baseUrl}IsExistedData`, { params });
  }
  getDropDownList(factory?: string) {
    let param: OvertimeApplicationMaintenance_Param = <OvertimeApplicationMaintenance_Param>{
      lang: this.language
    }
    if (factory)
      param.factory = factory
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  getShiftTime(data: OvertimeApplicationMaintenance_Main) {
    let param: OvertimeApplicationMaintenance_Param = <OvertimeApplicationMaintenance_Param>{
      factory: data.factory,
      work_Shift_Type: data.work_Shift_Type,
      overtime_Date_Str: data.overtime_Date_Str
    }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OvertimeApplicationMaintenance_Main>(` ${this.baseUrl}GetShiftTime`, { params });
  }
  getSearchDetail(
    param: Pagination,
    filter: OvertimeApplicationMaintenance_Param
  ): Observable<PaginationResult<OvertimeApplicationMaintenance_Main>> {
    filter.lang = this.language
    let params = new HttpParams().appendAll({ ...param, ...filter });
    return this.http.get<PaginationResult<OvertimeApplicationMaintenance_Main>>(
      `${this.baseUrl}GetSearchDetail`,
      { params }
    );
  }
  getOvertimeParam(data: OvertimeApplicationMaintenance_Main) {
    let param: OvertimeApplicationMaintenance_Param = <OvertimeApplicationMaintenance_Param>{
      factory: data.factory,
      work_Shift_Type: data.work_Shift_Type,
      overtime_Date_Str: data.overtime_Date_Str,
      overtime_Date_From_Str: data.overtime_Start_Str,
      overtime_Date_To_Str: data.overtime_End_Str,
    }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(`${this.baseUrl}GetOvertimeParam`, { params });
  }
  putData(data: OvertimeApplicationMaintenance_Main): Observable<OperationResult> {
    return this.http.put<OperationResult>(`${this.baseUrl}PutData`, data);
  }
  postData(data: OvertimeApplicationMaintenance_Main): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.baseUrl}PostData`, data);
  }
  deleteData(data: OvertimeApplicationMaintenance_Main) {
    return this.http.delete<OperationResult>(`${this.baseUrl}DeleteData`, { body: data });
  }
  downloadExcelTemplate() {
    return this.http.get<OperationResult>(` ${this.baseUrl}DownloadExcelTemplate`);
  }
  uploadExcel(file: FormData) {
    return this.http.post<OperationResult>(`${this.baseUrl}UploadExcel`, file);
  }
  export(params: OvertimeApplicationMaintenance_Param) {
    params.lang = this.language
    return this.http.get<OperationResult>(`${this.baseUrl}DownloadExcel`, { params: { ...params } });
  }
}
export const overtimeApplicationMaintenanceResolver: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_5_1_12_Overtime_Application_Maintenance).getDropDownList();
};
