import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import {
  WeeklyWorkingHoursReport_Basic,
  WeeklyWorkingHoursReportParam
} from '@models/attendance-maintenance/5_2_3_weekly-working-hours-report';
import { HttpClient, HttpParams } from '@angular/common/http';
import { OperationResult } from '@utilities/operation-result';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LocalStorageConstants } from '@constants/local-storage.constants';
@Injectable({
  providedIn: 'root'
})
export class S_5_2_3_WeeklyWorkingHoursReportService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_5_2_3_WeeklyWorkingHoursReport/`;

  //signal
  initData: WeeklyWorkingHoursReport_Basic = <WeeklyWorkingHoursReport_Basic>{
    param: <WeeklyWorkingHoursReportParam>{
      kind: "Department"
    },
    data: null,
  }

  paramSource = signal<WeeklyWorkingHoursReport_Basic>(structuredClone(this.initData));
  source$ = toObservable(this.paramSource);
  setSource = (data: WeeklyWorkingHoursReport_Basic) => this.paramSource.set(data)
  clearParams = () => {
    this.paramSource.set(structuredClone(this.initData))
  }
  constructor(private http: HttpClient) { }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }
  getLisDepartment(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params: { language: this.language, factory } });
  }
  getListLevel() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListLevel', { params: { language: this.language } });
  }
  download(param: WeeklyWorkingHoursReportParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadFileExcel', { params })
  }

  getCountRecords(param: WeeklyWorkingHoursReportParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<number>(this.apiUrl + 'GetCountRecords', { params })
  }
}
