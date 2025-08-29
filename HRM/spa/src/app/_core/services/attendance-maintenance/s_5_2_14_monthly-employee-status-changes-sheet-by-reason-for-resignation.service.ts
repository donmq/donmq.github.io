import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import {
  MonthlyEmployeeStatusChangesSheet_ByReasonForResignationParam,
  MonthlyEmployeeStatusChangesSheet_ByReasonForResignationResult
} from '@models/attendance-maintenance/5_2_14_monthly-employee-status-changes-sheet-by-reason-for-resignation';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_2_14_MonthlyEmployeeStatusChangesSheetByReasonForResignationService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl: string = `${environment.apiUrl}C_5_2_14_MonthlyEmployeeStatusChangesSheet_ByReasonforResignation`;
  initData: MonthlyEmployeeStatusChangesSheet_ByReasonForResignationParam = <MonthlyEmployeeStatusChangesSheet_ByReasonForResignationParam>{ permisionGroups: [] }
  baseInitSource = signal<MonthlyEmployeeStatusChangesSheet_ByReasonForResignationParam>(structuredClone(this.initData));
  setSource = (source: MonthlyEmployeeStatusChangesSheet_ByReasonForResignationParam) => this.baseInitSource.set(source);
  clearParams = () => { this.baseInitSource.set(structuredClone(this.initData)) }
  constructor(private http: HttpClient) { }

  //#region Get Dropdown
  getFactories() {
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}/GetFactories`, { params: { language: this.language } });
  }

  getPermistionGroups(factory: string) {
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}/GetPermistionGroups`, { params: { factory, language: this.language } });
  }
  //#endregion

  //#region Query Data + Export
  getTotalRecords(param: MonthlyEmployeeStatusChangesSheet_ByReasonForResignationParam) {
    param.language = this.language
    return this.http.post<MonthlyEmployeeStatusChangesSheet_ByReasonForResignationResult>(`${this.baseUrl}/GetTotalRecords`, param);
  }

  exportExcel(param: MonthlyEmployeeStatusChangesSheet_ByReasonForResignationParam) {
    param.language = this.language
    return this.http.post<OperationResult>(`${this.baseUrl}/Export`, param);
  }
  //#endregion
}
