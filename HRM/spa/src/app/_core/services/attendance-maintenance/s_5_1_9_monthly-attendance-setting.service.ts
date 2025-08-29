import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@env/environment';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import {
  HRMS_Att_Use_Monthly_LeaveDto,
  MonthlyAttendanceSettingParam_Main,
  ParamForm,
  ParamMain,
  ParamSource
} from '../../models/attendance-maintenance/5_1_9_monthly-attendance-setting';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { toObservable } from '@angular/core/rxjs-interop';
import { BehaviorSubject } from 'rxjs';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_1_9_MonthlyAttendanceSettingService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + "C_5_1_9_MonthlyAttendanceSetting/";

  initData: ParamSource = <ParamSource>{
    paramMain: <ParamMain>{
      data: [],
      pagination: <Pagination>{
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0
      },
      paramSearch: <MonthlyAttendanceSettingParam_Main>{}
    }
  }
  data = signal<ParamSource>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.data);
  setParamSearch = (data: ParamSource) => this.data.set(data)

  paramForm = new BehaviorSubject<ParamForm>(null);
  paramForm$ = this.paramForm.asObservable();
  setParamForm = (item: ParamForm) => this.paramForm.next(item);

  constructor(private http: HttpClient) { }

  clearParams() {
    this.data.set(structuredClone(this.initData))
    this.paramForm.next(null)
  }
  getDataPagination(pagination: Pagination, param: MonthlyAttendanceSettingParam_Main) {
    let params = new HttpParams().appendAll({ 'pageNumber': pagination.pageNumber, 'pageSize': pagination.pageSize, ...param });
    return this.http.get<PaginationResult<HRMS_Att_Use_Monthly_LeaveDto>>(this.apiUrl + "GetDataPagination", { params });
  }
  getFactorys() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + "GetFactorys", { params: { language: this.language } });
  }

  getLeaveTypes() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + "GetLeaveTypes", { params: { language: this.language } });
  }

  getAllowance() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + "GetAllowances", { params: { language: this.language } });
  }

  create(models: HRMS_Att_Use_Monthly_LeaveDto[]) {
    return this.http.post<OperationResult>(this.apiUrl + "Create", models);
  }

  edit(models: HRMS_Att_Use_Monthly_LeaveDto[]) {
    return this.http.put<OperationResult>(this.apiUrl + "Edit", models);
  }

  getCloneData(factory: string, leave_Type: string, effective_Month: string) {
    return this.http.get<HRMS_Att_Use_Monthly_LeaveDto[]>(this.apiUrl + "GetCloneData", { params: { factory, leave_Type, effective_Month } });
  }
  getRecentData(factory: string, effective_Month: string, leave_Type: string, action: string) {
    return this.http.get<HRMS_Att_Use_Monthly_LeaveDto[]>(this.apiUrl + "GetRecentData", { params: { factory, effective_Month, leave_Type, action } });
  }
  delete(factory: string, effective_Month: string) {
    let params = new HttpParams().appendAll({ 'factory': factory, 'effective_Month': effective_Month });
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { params });
  }

  checkDuplicateEffectiveMonth(factory: string, effective_Month: string) {
    let params = new HttpParams().appendAll({ 'factory': factory, 'effective_Month': effective_Month });
    return this.http.get<OperationResult>(this.apiUrl + "CheckDuplicateEffectiveMonth", { params });
  }


}
