import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { toObservable } from '@angular/core/rxjs-interop';
import { OperationResult } from '@utilities/operation-result';
import {
  DailyAttendancePostingBasic,
  DailyAttendancePostingParam
} from '@models/attendance-maintenance/5_1_17_daily-attendance-posting';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_1_17_DailyAttendancePostingService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + "C_5_1_17_DailyAttendancePosting/"
  initData: DailyAttendancePostingBasic = <DailyAttendancePostingBasic>{
    param: <DailyAttendancePostingParam>{},
    processedRecords: null
  }
  paramSource = signal<DailyAttendancePostingBasic>(structuredClone(this.initData));
  source$ = toObservable(this.paramSource);
  setSource = (data: DailyAttendancePostingBasic) => this.paramSource.set(data)
  clearParams = () => {
    this.paramSource.set(structuredClone(this.initData))
  }
  //#endregion
  constructor(private http: HttpClient) { }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }

  execute(param: DailyAttendancePostingParam) {
    return this.http.get<OperationResult>(this.apiUrl + 'Execute', { params: { factory: param.factory } });
  }

}
