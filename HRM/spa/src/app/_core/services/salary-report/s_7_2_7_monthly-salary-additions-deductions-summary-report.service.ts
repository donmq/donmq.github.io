import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { MonthlySalaryAdditionsDeductionsSummaryReportParam, MonthlySalaryAdditionsDeductionsSummaryReportSource } from '@models/salary-report/7_2_7_monthly-salary-additions-deductions-summary-report';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class S_7_2_7_MonthlySalaryAdditionsDeductionsSummaryReportService {
get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + "C_7_2_7_MonthlySalaryAdditionsDeductionsSummaryReport/"

  initData: MonthlySalaryAdditionsDeductionsSummaryReportSource = <MonthlySalaryAdditionsDeductionsSummaryReportSource>{
    param: <MonthlySalaryAdditionsDeductionsSummaryReportParam>{
      kind: 'OnJob',
      permission_Group: [],
    },
    totalRows: 0
  }

  programSource = signal<MonthlySalaryAdditionsDeductionsSummaryReportSource>(structuredClone(this.initData));
  programSource$ = toObservable(this.programSource);
  setSource = (source: MonthlySalaryAdditionsDeductionsSummaryReportSource) => this.programSource.set(source);
  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  }

  constructor(private http: HttpClient) { }

  getTotalRows(param: MonthlySalaryAdditionsDeductionsSummaryReportParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'GetTotalRows', { params })
  }

  download(param: MonthlySalaryAdditionsDeductionsSummaryReportParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'Download', { params });
  }

  getListFactory() {
    const language: string = localStorage.getItem(LocalStorageConstants.LANG)
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}GetListFactory`, { params: { language } });
  }
  
  getListPermissionGroup(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}GetListPermissionGroup`, { params });
  }

  getListDepartment(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + "GetListDepartment", { params: { factory, language: this.language } });
  }
}
