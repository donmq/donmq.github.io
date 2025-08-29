import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { HttpClient } from '@angular/common/http';
import {
  MonthlySalarySummaryReportParam,
  MonthlySalarySummaryReportSource
} from '@models/salary-report/7_2_4_monthly-salary-summary-report';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class S_7_2_4_MonthlySalarySummaryReportService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }

  constructor(private http: HttpClient) { }
  apiUrl = `${environment.apiUrl}C_7_2_4_MonthlySalarySummaryReport/`;

  initData = <MonthlySalarySummaryReportSource>{
    param: <MonthlySalarySummaryReportParam>{
      kind: 'Y',
      transfer: 'All',
      permission_Group: [],
      report_Kind: 'G'
    },
    totalRow: 0
  }
  paramSearch = signal<MonthlySalarySummaryReportSource>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: MonthlySalarySummaryReportSource) => this.paramSearch.set(data)

  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }

  getListPermissionGroup(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListPermissionGroup', { params: { factory, language: this.language } });
  }

  getListLevel() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListLevel', { params: { language: this.language } });
  }

  getListDepartment(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params: { factory, language: this.language } });
  }

  getTotal(param: MonthlySalarySummaryReportParam) {
    param.lang = this.language
    return this.http.get<number>(this.apiUrl + 'GetTotal', { params: { ...param } })
  }

  downloadExcel(params: MonthlySalarySummaryReportParam) {
    params.lang = this.language
    return this.http.get<OperationResult>(`${this.apiUrl}DownloadExcel`, { params: { ...params } });
  }
}
