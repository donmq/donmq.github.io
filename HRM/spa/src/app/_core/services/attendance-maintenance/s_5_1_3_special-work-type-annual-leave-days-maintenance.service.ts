import { Injectable, signal } from '@angular/core';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { UserForLogged } from '@models/auth/auth';
import { toObservable } from '@angular/core/rxjs-interop';
import {
  HRMS_Att_Work_Type_DaysDto,
  SpecialWorkTypeAnnualLeaveDaysMaintenanceParam,
  SpecialWorkTypeAnnualLeaveDaysMaintenanceSource
} from '@models/attendance-maintenance/5_1_3_special-work-type-annual-leave-days-maintenance'
import { KeyValuePair } from '@utilities/key-value-pair';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root'
})
export class S_5_1_3_SpecialWorkTypeAnnualLeaveDaysMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  user: UserForLogged = JSON.parse((localStorage.getItem(LocalStorageConstants.USER)));
  initData: SpecialWorkTypeAnnualLeaveDaysMaintenanceSource = <SpecialWorkTypeAnnualLeaveDaysMaintenanceSource>{
    param: <SpecialWorkTypeAnnualLeaveDaysMaintenanceParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: null
  }

  constructor(private http: HttpClient) { }
  apiUrl = `${environment.apiUrl}C_5_1_3_SpecialWorkTypeAnnualLeaveDaysMaintenance/`;
  paramSource = signal<SpecialWorkTypeAnnualLeaveDaysMaintenanceSource>(structuredClone(this.initData));
  source$ = toObservable(this.paramSource);
  setSource = (data: SpecialWorkTypeAnnualLeaveDaysMaintenanceSource) => this.paramSource.set(data)
  clearParams = () => {
    this.paramSource.set(structuredClone(this.initData))
  }

  getData(pagination: PaginationParam, param: SpecialWorkTypeAnnualLeaveDaysMaintenanceParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<HRMS_Att_Work_Type_DaysDto>>(this.apiUrl + "GetData", { params })
  }

  addNew(param: HRMS_Att_Work_Type_DaysDto) {
    return this.http.post<OperationResult>(`${this.apiUrl}AddNew`, param);
  }

  edit(param: HRMS_Att_Work_Type_DaysDto) {
    return this.http.put<OperationResult>(this.apiUrl + "Edit", param)
  }

  delete(model: HRMS_Att_Work_Type_DaysDto) {
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { params: {}, body: model });
  }

  getListDivision() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDivision', { params: { language: this.language } });
  }

  getListWorkType() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListWorkType', { params: { language: this.language } });
  }

  getListFactory(division: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { division, language: this.language } });
  }
  downloadExcel(param: SpecialWorkTypeAnnualLeaveDaysMaintenanceParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadExcel', { params });
  }


}
