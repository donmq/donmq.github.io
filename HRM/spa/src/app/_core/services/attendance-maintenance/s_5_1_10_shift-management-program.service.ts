import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { toObservable } from '@angular/core/rxjs-interop';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { environment } from '@env/environment';
import { BehaviorSubject, Observable } from 'rxjs';
import { OperationResult } from '@utilities/operation-result';
import {
  ShiftManagementProgram_Main,
  ShiftManagementProgram_MainMemory,
  ShiftManagementProgram_Param,
  ShiftManagementProgram_Update,
  TypeheadKeyValue
} from '@models/attendance-maintenance/5_1_10_shift-management-program';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root',
})

export class S_5_1_10_ShiftManagementProgram implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_5_1_10_ShiftManagementProgram/`;
  initData: ShiftManagementProgram_MainMemory = <ShiftManagementProgram_MainMemory>{
    param: <ShiftManagementProgram_Param>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<ShiftManagementProgram_MainMemory>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: ShiftManagementProgram_MainMemory) => this.paramSearch.set(data)

  paramForm = new BehaviorSubject<ShiftManagementProgram_Main>(null);
  paramForm$ = this.paramForm.asObservable();
  setParamForm = (item: ShiftManagementProgram_Main) => this.paramForm.next(item);

  constructor(
    private http: HttpClient
  ) { }
  clearParams() {
    this.paramSearch.set(structuredClone(this.initData))
    this.paramForm.next(null)
  }
  isExistedData(data: ShiftManagementProgram_Main) {
    let param: ShiftManagementProgram_Param = <ShiftManagementProgram_Param>{
      division: data.division,
      factory: data.factory,
      effective_Date: data.effective_Date,
      effective_Date_Str: data.effective_Date_Str
    }
    if (data.employee_Id) {
      param.employee_Id = data.employee_Id
      param.useR_GUID = data.useR_GUID
    }
    if (data.department)
      param.department = data.department
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(` ${this.baseUrl}IsExistedData`, { params });
  }

  getDropDownList(division?: string) {
    let param: ShiftManagementProgram_Param = <ShiftManagementProgram_Param>{
      lang: this.language
    }
    if (division)
      param.division = division
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  getDepartmentList(division: string, factory: string) {
    let param: ShiftManagementProgram_Param = <ShiftManagementProgram_Param>{
      division: division,
      factory: factory,
      lang: this.language
    }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDepartmentList`, { params });
  }
  getWorkShiftTypeDepartmentList(division: string, factory: string, department: string) {
    let param: ShiftManagementProgram_Param = <ShiftManagementProgram_Param>{
      division: division,
      factory: factory,
      department: department,
      lang: this.language
    }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetWorkShiftTypeDepartmentList`, { params });
  }
  getSearchDetail(
    param: Pagination,
    filter: ShiftManagementProgram_Param
  ): Observable<PaginationResult<ShiftManagementProgram_Main>> {
    filter.lang = this.language
    let params = new HttpParams().appendAll({ ...param, ...filter });
    return this.http.get<PaginationResult<ShiftManagementProgram_Main>>(
      `${this.baseUrl}GetSearchDetail`,
      { params }
    );
  }
  getEmployeeList(division: string, factory: string, employee_Id: string) {
    let param: ShiftManagementProgram_Param = <ShiftManagementProgram_Param>{
      division: division,
      factory: factory,
      employee_Id: employee_Id,
      lang: this.language
    }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<TypeheadKeyValue[]>(` ${this.baseUrl}GetEmployeeList`, { params });
  }
  getEmployeeDetail(division: string, factory: string, department: string) {
    let param: ShiftManagementProgram_Param = <ShiftManagementProgram_Param>{
      division: division,
      factory: factory,
      department: department,
      lang: this.language
    }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<ShiftManagementProgram_Main[]>(` ${this.baseUrl}GetEmployeeDetail`, { params });
  }
  postDataEmployee(data: ShiftManagementProgram_Main): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.baseUrl}PostDataEmployee`, data);
  }
  postDataDepartment(data: ShiftManagementProgram_Main[]): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.baseUrl}PostDataDepartment`, data);
  }
  putDataEmployee(data: ShiftManagementProgram_Main, temp: ShiftManagementProgram_Main): Observable<OperationResult> {
    const inputData: ShiftManagementProgram_Update = <ShiftManagementProgram_Update>{
      temp_Data: temp,
      recent_Data: data
    }
    return this.http.put<OperationResult>(`${this.baseUrl}PutDataEmployee`, inputData);
  }
  batchDelete(data: ShiftManagementProgram_Main[]) {
    return this.http.delete<OperationResult>(`${this.baseUrl}BatchDelete`, { body: data });
  }
}
export const shiftManagementProgramResolver: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_5_1_10_ShiftManagementProgram).getDropDownList();
};
