import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import {
  DailyUnregisteredOvertimeList_Memory,
  DailyUnregisteredOvertimeList_Param
} from '@models/attendance-maintenance/5_2_7_daily_unregistered_overtime_list';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root',
})

export class S_5_2_7_DailyUnregisteredOvertimeList implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_5_2_7_DailyUnregisteredOvertime/`;
  initData: DailyUnregisteredOvertimeList_Memory = <DailyUnregisteredOvertimeList_Memory>{
    param: <DailyUnregisteredOvertimeList_Param>{},
    total: 0
  }
  paramSearch = signal<DailyUnregisteredOvertimeList_Memory>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: DailyUnregisteredOvertimeList_Memory) => this.paramSearch.set(data)
  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  constructor(
    private http: HttpClient
  ) { }

  getDropDownList(factory?: string) {
    let param: DailyUnregisteredOvertimeList_Param = <DailyUnregisteredOvertimeList_Param>{
      lang: this.language
    }
    if (factory)
      param.factory = factory
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  search(param: DailyUnregisteredOvertimeList_Param) {
    param.lang = this.language
    var params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(`${this.baseUrl}Search`, { params });
  }
  excel(param: DailyUnregisteredOvertimeList_Param) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(`${this.baseUrl}Excel`, { params });
  }
}
export const dailyUnregisteredResolver: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_5_2_7_DailyUnregisteredOvertimeList).getDropDownList();
};
