import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import {
  DailyNoNightShiftHoursList_Memory,
  DailyNoNightShiftHoursList_Param
} from '@models/attendance-maintenance/5_2_8_daily_no_night_shift_hours_list';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root',
})

export class S_5_2_8_DailyNoNightShiftHoursList implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_5_2_8_DailyNoNightShiftHoursList/`;
  initData: DailyNoNightShiftHoursList_Memory = <DailyNoNightShiftHoursList_Memory>{
    param: <DailyNoNightShiftHoursList_Param>{},
    total: 0
  }
  paramSearch = signal<DailyNoNightShiftHoursList_Memory>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: DailyNoNightShiftHoursList_Memory) => this.paramSearch.set(data)
  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }
  constructor(
    private http: HttpClient
  ) { }

  getDropDownList(factory?: string) {
    let param: DailyNoNightShiftHoursList_Param = <DailyNoNightShiftHoursList_Param>{
      lang: this.language
    }
    if (factory)
      param.factory = factory
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  search(param: DailyNoNightShiftHoursList_Param) {
    param.lang = this.language
    var params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(`${this.baseUrl}Search`, { params });
  }
  excel(param: DailyNoNightShiftHoursList_Param) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(`${this.baseUrl}Excel`, { params });
  }
}
export const dailyNoNightResolver: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_5_2_8_DailyNoNightShiftHoursList).getDropDownList();
};
