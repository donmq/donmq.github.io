import { OperationResult } from '@utilities/operation-result';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { EmployeeRewardAndPenaltyReportParam, EmployeeRewardAndPenaltyReportSource } from '@models/reward-and-penalty-report/8_2_1_employee-reward-and-penalty-report';
import { KeyValuePair } from '@utilities/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class S_8_2_1_EmployeeRewardAndPenaltyReportService {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + 'C_8_2_1_EmployeeRewardAndPenaltyReport/';

  initData: EmployeeRewardAndPenaltyReportSource = <EmployeeRewardAndPenaltyReportSource>{
    param: <EmployeeRewardAndPenaltyReportParam>{
      permission_Group: [],
      counts: '1'
    },
    totalRows: 0,
    start_Date: null,
    end_Date: null,
    start_Year_Month: null,
    end_Year_Month: null
  }

  programSource = signal<EmployeeRewardAndPenaltyReportSource>(structuredClone(this.initData));
  programSource$ = toObservable(this.programSource);
  setSource = (source: EmployeeRewardAndPenaltyReportSource) => this.programSource.set(source);

  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  }
  constructor(private _http: HttpClient) { }

  getTotalRows(param: EmployeeRewardAndPenaltyReportParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this._http.get<OperationResult>(this.apiUrl + 'GetTotalRows', { params })
  }

  download(param: EmployeeRewardAndPenaltyReportParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this._http.get<OperationResult>(this.apiUrl + 'Download', { params });
  }

  getListFactory() {
    const language: string = localStorage.getItem(LocalStorageConstants.LANG)
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListFactory`, { params: { language } });
  }

  getListPermissionGroup(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListPermissionGroup`, { params });
  }

  getListDepartment(factory: string) {
    return this._http.get<KeyValuePair[]>(this.apiUrl + "GetListDepartment", { params: { factory, language: this.language } });
  }

  getListRewardPenalty() {
    return this._http.get<KeyValuePair[]>(this.apiUrl + "GetListRewardPenalty", { params: { language: this.language } });
  }
}
