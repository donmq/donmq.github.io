import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { HttpClient } from '@angular/common/http';
import {
  MonthlySalaryDetailReportParam,
  MonthlySalaryDetailReportSource
} from '@models/salary-report/7_2_5_monthly-salary-detail-report';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class S_7_2_5_MonthlySalaryDetailReportService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }

  constructor(private http: HttpClient) { }
  apiUrl = `${environment.apiUrl}C_7_2_5_MonthlySalaryDetailReport/`;

  initData = <MonthlySalaryDetailReportSource>{
    param: <MonthlySalaryDetailReportParam>{
      kind: "Y",
      transfer: "All",
      permission_Group: []
    },
    totalRow: 0
  }
  paramSearch = signal<MonthlySalaryDetailReportSource>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: MonthlySalaryDetailReportSource) => this.paramSearch.set(data)

  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }

  getListPermissionGroup(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListPermissionGroup', { params: { factory, language: this.language } });
  }

  getListDepartment(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params: { factory, language: this.language } });
  }

  getTotal(param: MonthlySalaryDetailReportParam) {
    param.lang = this.language;
    return this.http.get<number>(this.apiUrl + 'GetTotal', { params: { ...param } });
  }

  downloadExcel(params: MonthlySalaryDetailReportParam) {
    params.lang = this.language;
    return this.http.get<OperationResult>(`${this.apiUrl}DownloadExcel`, { params: { ...params } });
  }
}
