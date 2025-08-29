import {
  EmployeeLunchBreakTimeSettingParam,
  EmployeeLunchBreakTimeSettingSource,
  HRMS_Att_LunchtimeDto
} from '@models/attendance-maintenance/5_1_6_employee-lunch-break-time-setting';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { KeyValuePair } from '@utilities/key-value-pair';
import { Injectable, signal } from '@angular/core';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_1_6_EmployeeLunchBreakTimeSettingService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  constructor(private http: HttpClient) { }
  apiUrl = `${environment.apiUrl}C_5_1_6_EmployeeLucnhBreakTimeSetting/`;

  initData: EmployeeLunchBreakTimeSettingSource = <EmployeeLunchBreakTimeSettingSource>{
    param: <EmployeeLunchBreakTimeSettingParam>{},
    selectedKey: 'Y',
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<EmployeeLunchBreakTimeSettingSource>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: EmployeeLunchBreakTimeSettingSource) => this.paramSearch.set(data)
  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }
  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }

  getListDepartment(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params: { factory, language: this.language } });
  }

  getData(pagination: PaginationParam, param: EmployeeLunchBreakTimeSettingParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<HRMS_Att_LunchtimeDto>>(this.apiUrl + "GetData", { params })
  }

  uploadExcel(file: FormData) {
    return this.http.post<OperationResult>(this.apiUrl + 'UploadExcel', file)
  }

  downloadTemplate() {
    return this.http.get<OperationResult>(this.apiUrl + "DownloadExcelTemplate");
  }

  delete(model: HRMS_Att_LunchtimeDto) {
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { params: {}, body: model });
  }
  downloadExcel(param: EmployeeLunchBreakTimeSettingParam) {
    param.lang = this.language;
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadExcel', { params });
  }
}
