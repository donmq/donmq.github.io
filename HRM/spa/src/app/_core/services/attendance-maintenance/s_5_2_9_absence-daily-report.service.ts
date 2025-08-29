import { Injectable, signal } from '@angular/core';
import { OperationResult } from '@utilities/operation-result';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { UserForLogged } from '@models/auth/auth';
import {
  AbsenceDailyReportCount,
  AbsenceDailyReportParam,
  AbsenceDailyReportSource
} from '@models/attendance-maintenance/5_2_9_absence-daily-report';
import { KeyValuePair } from '@utilities/key-value-pair';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
@Injectable({
  providedIn: 'root'
})
export class S_5_2_9_AbsenceDailyReportService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  user: UserForLogged = JSON.parse((localStorage.getItem(LocalStorageConstants.USER)));
  constructor(private http: HttpClient) { }
  apiUrl = `${environment.apiUrl}C_5_2_9_AbsenceDailyReport/`;
  initData: AbsenceDailyReportSource = <AbsenceDailyReportSource>{
    param: <AbsenceDailyReportParam>{},
    resultCount: <AbsenceDailyReportCount>{}
  }
  programSource = signal<AbsenceDailyReportSource>(structuredClone(this.initData));
  programSource$ = toObservable(this.programSource);
  setSource = (source: AbsenceDailyReportSource) => this.programSource.set(source);
  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  }

  getTotalRows(param: AbsenceDailyReportParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<AbsenceDailyReportCount>(this.apiUrl + 'GetTotalRows', { params })
  }

  downloadExcel(param: AbsenceDailyReportParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadExcel', { params });
  }

  getListFactory() {
    let params = new HttpParams().appendAll({ lang: this.language })
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params });
  }
}
