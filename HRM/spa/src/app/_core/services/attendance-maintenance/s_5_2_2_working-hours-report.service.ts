import { environment } from '@env/environment';
import { toObservable } from '@angular/core/rxjs-interop';
import { OperationResult } from '@utilities/operation-result';
import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import {
  WorkingHoursReportParam,
  WorkingHoursReportSource
} from '@models/attendance-maintenance/5_2_2_working_hours_report';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_2_2_WorkingHoursReportService implements IClearCache {

  constructor(private http: HttpClient) { }
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_5_2_2_WorkingHoursReport/`;

  initData = <WorkingHoursReportSource>{
    dateFrom: null,
    dateTo: null,
    param: <WorkingHoursReportParam>{
      factory: '',
      department: '',
    },
    total: 0
  };
  paramSearch = signal<WorkingHoursReportSource>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: WorkingHoursReportSource) => this.paramSearch.set(data)

  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }

  getListDepartment(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params: { factory, language: this.language } });
  }

  getTotal(param: WorkingHoursReportParam) {
    param.lang = this.language
    return this.http.get<number>(this.apiUrl + 'GetTotal', { params: { ...param } })
  }

  downloadExcel(param: WorkingHoursReportParam) {
    param.lang = this.language
    return this.http.get<OperationResult>(this.apiUrl + "DownloadExcel", { params: { ...param } });
  }
}

