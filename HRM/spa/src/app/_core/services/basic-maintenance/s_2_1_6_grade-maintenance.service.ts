import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { HRMS_Basic_Level, GradeMaintenanceParam, ParamInMain } from '@models/basic-maintenance/2_1_6_grade-maintenance';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { OperationResult } from '@utilities/operation-result';
import { KeyValuePair } from '@utilities/key-value-pair';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';
@Injectable({
  providedIn: 'root'
})
export class S_2_1_6_GradeMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) };
  apiUrl: string = environment.apiUrl + "C_2_1_6_LevelMaintenance/";

  initDataSource: HRMS_Basic_Level = <HRMS_Basic_Level>{}
  data = signal<HRMS_Basic_Level>(JSON.parse(JSON.stringify(this.initDataSource)));
  data$ = toObservable(this.data);
  setData = (_data: HRMS_Basic_Level) => this.data.set(_data)

  initDataParam: ParamInMain = <ParamInMain>{
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    param: <GradeMaintenanceParam>{},
    data: []
  }
  getParamInMain = signal<ParamInMain>(JSON.parse(JSON.stringify(this.initDataParam)));
  getParamInMain$ = toObservable(this.getParamInMain);
  setParamInMain = (_data: ParamInMain) => this.getParamInMain.set(_data)

  constructor(private http: HttpClient) { }

  clearParams() {
    this.data.set(JSON.parse(JSON.stringify(this.initDataSource)))
    this.getParamInMain.set(this.initDataParam)
  }

  getData(pagination: Pagination, param: GradeMaintenanceParam): Observable<PaginationResult<HRMS_Basic_Level>> {
    param.language = this.language
    let params = new HttpParams().appendAll({ 'pageNumber': pagination.pageNumber, 'pageSize': pagination.pageSize, ...param });
    return this.http.get<PaginationResult<HRMS_Basic_Level>>(this.apiUrl + "GetData", { params });
  }

  add(model: HRMS_Basic_Level): Observable<OperationResult> {
    return this.http.post<OperationResult>(this.apiUrl + "Add", model);
  }

  edit(model: HRMS_Basic_Level): Observable<OperationResult> {
    return this.http.put<OperationResult>(this.apiUrl + "Edit", model);
  }

  delete(model: HRMS_Basic_Level): Observable<OperationResult> {
    let params = new HttpParams().appendAll({ ...model });
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { params });
  }

  getListLevelCode(type: string): Observable<KeyValuePair[]> {
    return this.http.get<KeyValuePair[]>(this.apiUrl + "GetListLevelCode", { params: { type, language: this.language } });
  }

  exportExcel(param: GradeMaintenanceParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param, lang: this.language });
    return this.http.get<OperationResult>(this.apiUrl + "ExportExcel", { params });
  }

  getTypes(): Observable<KeyValuePair[]> {
    return this.http.get<KeyValuePair[]>(this.apiUrl + "GetTypes", { params: { language: this.language } });
  }

}
