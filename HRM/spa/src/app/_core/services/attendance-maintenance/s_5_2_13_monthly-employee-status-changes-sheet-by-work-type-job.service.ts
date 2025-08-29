import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { MonthlyEmployeeStatusChangesSheet_ByWorkTypeJobParam } from '@models/attendance-maintenance/5_2_13_monthly-employee-status-changes-sheet-by-work-type-job';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_2_13_MonthlyEmployeeStatusChangesSheetByWorkTypeJobService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl: string = `${environment.apiUrl}C_5_2_13_MonthlyEmployeeStatusChangesSheet_ByWorkTypeJob`;
  initData: MonthlyEmployeeStatusChangesSheet_ByWorkTypeJobParam = <MonthlyEmployeeStatusChangesSheet_ByWorkTypeJobParam>{
    permisionGroup: [],
    work_Type: []
  }
  baseInitSource = signal<MonthlyEmployeeStatusChangesSheet_ByWorkTypeJobParam>(structuredClone(this.initData));
  setSource = (source: MonthlyEmployeeStatusChangesSheet_ByWorkTypeJobParam) => this.baseInitSource.set(source);
  clearParams = () => { this.baseInitSource.set(structuredClone(this.initData)) }
  constructor(private http: HttpClient) { }

  //#region Get Dropdown
  getFactories() {
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}/GetFactories`, { params: { language: this.language } });
  }

  getLevels() {
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}/GetLevels`, { params: { language: this.language } });
  }

  getPermistionGroups(factory: string) {
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}/GetPermistionGroups`, { params: { factory, language: this.language } });
  }

  getWorkTypeJobs() {
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}/GetWorkTypeJobs`, { params: { language: this.language } });
  }
  //#endregion

  //#region Query Data + Export
  getTotalRecords(param: MonthlyEmployeeStatusChangesSheet_ByWorkTypeJobParam) {
    param.language = this.language
    return this.http.post<OperationResult>(`${this.baseUrl}/GetTotalRecords`, param);
  }

  exportExcel(param: MonthlyEmployeeStatusChangesSheet_ByWorkTypeJobParam) {
    param.language = this.language
    return this.http.post<OperationResult>(`${this.baseUrl}/Export`, param);
  }
  //#endregion


}
