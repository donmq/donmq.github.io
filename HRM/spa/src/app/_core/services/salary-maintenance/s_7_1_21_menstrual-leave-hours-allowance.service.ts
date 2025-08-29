import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { MenstrualLeaveHoursAllowanceParam, MenstrualLeaveHoursAllowanceSource } from '@models/salary-maintenance/7_1_21_menstrual-leave-hours-allowance';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_21_MenstrualLeaveHoursAllowanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + 'C_7_1_21_MenstrualLeaveHoursAllowance/';

  initData: MenstrualLeaveHoursAllowanceSource = <MenstrualLeaveHoursAllowanceSource>{
    param: <MenstrualLeaveHoursAllowanceParam>{},
    totalRows: 0,
  }

  programSource = signal<MenstrualLeaveHoursAllowanceSource>(structuredClone(this.initData));
  programSource$ = toObservable(this.programSource);
  setSource = (data: MenstrualLeaveHoursAllowanceSource) => this.programSource.set(data)

  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  }

  constructor(private _http: HttpClient) { }

  checkData(param: MenstrualLeaveHoursAllowanceParam) {
    return this._http.get<OperationResult>(`${this.apiUrl}CheckData`, { params: { ...param } });
  }

  execute(param: MenstrualLeaveHoursAllowanceParam) {
    return this._http.put<OperationResult>(`${this.apiUrl}Execute`, param);
  }

  getListFactory() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListFactory`, { params: { language: this.language } });
  }

  getListPermissionGroup(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListPermissionGroup`, { params });
  }
}
