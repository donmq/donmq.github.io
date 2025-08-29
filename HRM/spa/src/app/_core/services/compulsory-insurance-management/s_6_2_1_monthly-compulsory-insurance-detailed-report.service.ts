import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { toObservable } from '@angular/core/rxjs-interop';
import { MonthlyCompulsoryInsuranceDetailedReportSource, MonthlyCompulsoryInsuranceDetailedReportParam } from '@models/compulsory-insurance-management/6_1_5_monthly-compulsory-insurance-detailed-report';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_6_2_1_MonthlyCompulsoryInsuranceDetailedReportService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + "C_6_2_1_MonthlyCompulsoryInsuranceDetailedReport/"

  initData: MonthlyCompulsoryInsuranceDetailedReportSource = <MonthlyCompulsoryInsuranceDetailedReportSource>{
    param: <MonthlyCompulsoryInsuranceDetailedReportParam>{
      permission_Group: [],
      permission_Group_Name: [],
      kind: 'O'
    },
    totalRows: 0
  }
  programSource = signal<MonthlyCompulsoryInsuranceDetailedReportSource>(structuredClone(this.initData));
  programSource$ = toObservable(this.programSource);
  setSource = (source: MonthlyCompulsoryInsuranceDetailedReportSource) => this.programSource.set(source);
  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  }

  constructor(private http: HttpClient) { }
  getTotalRows(param: MonthlyCompulsoryInsuranceDetailedReportParam) {
    param.language = this.language
    const params = new HttpParams().appendAll({ ...param });
    return this.http.get<number>(this.apiUrl + 'GetTotalRows', { params })
  }
  downloadExcel(param: MonthlyCompulsoryInsuranceDetailedReportParam) {
    param.language = this.language
    const params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadExcel', { params });
  }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language : this.language  } });
  }

  getListInsuranceType() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListInsuranceType', { params: { language : this.language  } });
  }

  getListPermissionGroupByFactory(factory: string) {
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}GetListPermissionGroupByFactory`, {  params: { factory, language : this.language } });
  }

  getDepartments(factory: string) {
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}GetDepartments`, {
      params: { factory, language : this.language },
    });
  }

}
