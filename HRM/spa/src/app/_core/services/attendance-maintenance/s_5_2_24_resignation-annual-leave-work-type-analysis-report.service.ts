import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import {
  ResignationAnnualLeaveWorktypeAnalysisReportParam,
  ResignationAnnualLeaveWorktypeAnalysisReportSource
} from '@models/attendance-maintenance/5_2_24_resignation-annual-leave-work-type-analysis-report';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_2_24_ResignationAnnualLeaveWorktypeAnalysisReportService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + "C_5_2_24_ResignationAnnualLeaveWorktypeAnalysisReport/"

  initData: ResignationAnnualLeaveWorktypeAnalysisReportSource = <ResignationAnnualLeaveWorktypeAnalysisReportSource>{
    param: <ResignationAnnualLeaveWorktypeAnalysisReportParam>{
      permission_Group: []
    },
    totalRows: 0
  }
  programSource = signal<ResignationAnnualLeaveWorktypeAnalysisReportSource>(structuredClone(this.initData));
  programSource$ = toObservable(this.programSource);
  setSource = (source: ResignationAnnualLeaveWorktypeAnalysisReportSource) => this.programSource.set(source);
  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  }

  constructor(private http: HttpClient) { }

  search(param: ResignationAnnualLeaveWorktypeAnalysisReportParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<number>(this.apiUrl + 'Search', { params })
  }

  download(param: ResignationAnnualLeaveWorktypeAnalysisReportParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'Download', { params });
  }

  getListFactory() {
    let params = new HttpParams().appendAll({ language: this.language })
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params });
  }

  getListLevel() {
    let params = new HttpParams().appendAll({ language: this.language })
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListLevel', { params });
  }

  getListPermissionGroup(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListPermissionGroup', { params });
  }
}
