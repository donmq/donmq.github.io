import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { MonthlyCompulsoryInsuranceSummaryReport_Source, MonthlyCompulsoryInsuranceSummaryReport_Param } from '@models/compulsory-insurance-management/6_1_6_monthly-compulsory-insurance-summary-report';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class S_6_2_2_MonthlyCompulsoryInsuranceSummaryReportService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_6_2_2_MonthlyCompulsoryInsuranceSummaryReport/`;
  initData: MonthlyCompulsoryInsuranceSummaryReport_Source = <MonthlyCompulsoryInsuranceSummaryReport_Source>{
    param: <MonthlyCompulsoryInsuranceSummaryReport_Param>{
      kind: "On Job",
      permission_Group: [],
      permission_Group_Name: []
    },
    data: null,
    total: 0
  }
  paramSearch = signal<MonthlyCompulsoryInsuranceSummaryReport_Source>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: MonthlyCompulsoryInsuranceSummaryReport_Source) => this.paramSearch.set(data)

  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  constructor(private http: HttpClient) { }
  getListFactory() {
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}GetFactoryList`, { params: { language: this.language } })
  }
  getDepartmentList(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetDepartmentList', { params: { factory, language: this.language } });
  }

  getPermissionGroup(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetPermissionGroup', { params: { factory, language: this.language } });
  }

  getListInsuranceType() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListInsuranceType', { params: { language: this.language } });
  }

  getCountRecords(param: MonthlyCompulsoryInsuranceSummaryReport_Param) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param })
    return this.http.get<number>(this.apiUrl + 'GetCountRecords', { params })
  }

  downloadExcel(param: MonthlyCompulsoryInsuranceSummaryReport_Param) {
    param.language = this.language
    const params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadExcel', { params });
  }


}
