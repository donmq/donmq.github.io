import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient } from '@angular/common/http';
import { KeyValuePair } from '@utilities/key-value-pair';
import { NightShiftSubsidyMaintenance_Param, NightShiftSubsidyMaintenanceSource } from '@models/salary-maintenance/7_1_20-night-shift-subsidy-maintenance';
import { OperationResult } from '@utilities/operation-result';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';
@Injectable({
  providedIn: 'root'
})
export class S_7_1_20_NightShiftSubsidyMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_7_1_20_NightShiftSubsidyMaintenance/`;
  initData: NightShiftSubsidyMaintenanceSource = <NightShiftSubsidyMaintenanceSource>{
    param: <NightShiftSubsidyMaintenance_Param>{
      permission: []
    },
    processRecords: 0,
  }
  programSource = signal<NightShiftSubsidyMaintenanceSource>(structuredClone(this.initData));
  programSource$ = toObservable(this.programSource);
  setSource = (data: NightShiftSubsidyMaintenanceSource) => this.programSource.set(data)

  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  }

  constructor(private _http: HttpClient) { }
  getFactories() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetFactory`, { params: { language: this.language } });
  }

  getPermissionGroup(factory: string) {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetPermissionGroup`, { params: { factory, language: this.language } });
  }

  excute(param: NightShiftSubsidyMaintenance_Param) {
    return this._http.post<OperationResult>(`${this.baseUrl}Excute`, param)
  }
  checkData(param: NightShiftSubsidyMaintenance_Param) {
    return this._http.get<OperationResult>(`${this.baseUrl}CheckData`, { params: { ...param } });
  }
}
