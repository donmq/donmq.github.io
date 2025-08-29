import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import { OperationResult } from '@utilities/operation-result';
import { FactoryResignationAnalysisReport_Param } from '@models/attendance-maintenance/5_2_23_factory_resignation_analysis_report';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root',
})

export class S_5_2_23_FactoryResignationAnalysisReport implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_5_2_23_FactoryResignationAnalysisReport/`;
  initData: FactoryResignationAnalysisReport_Param = <FactoryResignationAnalysisReport_Param>{
    total_Rows: 0
  }
  paramSearch = signal<FactoryResignationAnalysisReport_Param>(structuredClone(this.initData))
  paramSearch$ = toObservable(this.paramSearch);

  setParamSearch = (data: FactoryResignationAnalysisReport_Param) => this.paramSearch.set(data)
  clearParams() {
    this.paramSearch.set(structuredClone(this.initData))
  }

  constructor(
    private http: HttpClient
  ) { }
  getDropDownList(param: FactoryResignationAnalysisReport_Param) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  process(param: FactoryResignationAnalysisReport_Param) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(`${this.baseUrl}Process`, { params });
  }
}
