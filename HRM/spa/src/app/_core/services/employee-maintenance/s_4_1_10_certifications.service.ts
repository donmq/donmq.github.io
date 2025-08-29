import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { ResolveFn } from '@angular/router';
import {
  Certifications_SubModel,
  Certifications_MainData,
  Certifications_MainMemory,
  Certifications_MainParam,
  Certifications_SubMemory,
  Certifications_SubParam,
  Certifications_DownloadFileModel,
  Certifications_TypeheadKeyValue
} from '@models/employee-maintenance/4_1_10_certifications';
import { toObservable } from '@angular/core/rxjs-interop';
import { BehaviorSubject, Observable } from 'rxjs';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { environment } from '@env/environment';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root',
})

export class S_4_1_10_Certifications implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_4_1_10_Certifications/`;
  initData: Certifications_MainMemory = <Certifications_MainMemory>{
    param: <Certifications_MainParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<Certifications_MainMemory>(JSON.parse(JSON.stringify(this.initData)));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: Certifications_MainMemory) => this.paramSearch.set(data)

  paramForm = new BehaviorSubject<Certifications_SubParam>(null);
  paramForm$ = this.paramForm.asObservable();
  setParamForm = (item: Certifications_SubParam) => this.paramForm.next(item);

  constructor(private http: HttpClient) { }

  clearParams() {
    this.paramSearch.set(JSON.parse(JSON.stringify(this.initData)))
    this.paramForm.next(null)
  }
  getDropDownList(division?: string) {
    let param: Certifications_MainParam = <Certifications_MainParam>{ lang: this.language }
    if (division)
      param.division = division
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  getEmployeeList(division: string, factory: string, employee_Id: string) {
    let param: Certifications_SubParam = <Certifications_SubParam>{
      division: division,
      factory: factory,
      employee_Id: employee_Id
    }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<Certifications_TypeheadKeyValue[]>(` ${this.baseUrl}GetEmployeeList`, { params });
  }
  getSearchDetail(param: Pagination, filter: Certifications_MainParam): Observable<PaginationResult<Certifications_MainData>> {
    filter.lang = this.language
    let params = new HttpParams().appendAll({ ...param, ...filter });
    return this.http.get<PaginationResult<Certifications_MainData>>(
      `${this.baseUrl}GetSearchDetail`, { params }
    );
  }
  getSubDetail(param: Certifications_SubParam) {
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(` ${this.baseUrl}GetSubDetail`, { params });
  }
  checkExistedData(data: Certifications_SubModel) {
    let params = new HttpParams().appendAll({ ...data });
    return this.http.get<OperationResult>(` ${this.baseUrl}CheckExistedData`, { params });
  }
  downloadFile(data: Certifications_DownloadFileModel): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.baseUrl}DownloadFile`, data);
  }
  putData(data: Certifications_SubMemory): Observable<OperationResult> {
    return this.http.put<OperationResult>(`${this.baseUrl}PutData`, data);
  }
  postData(data: Certifications_SubMemory): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.baseUrl}PostData`, data);
  }
  deleteData(data: Certifications_SubModel) {
    let params = new HttpParams().appendAll({ ...data })
    return this.http.delete<OperationResult>(`${this.baseUrl}DeleteData`, { params });
  }
  downloadExcel(param: Certifications_MainParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(` ${this.baseUrl}DownloadExcel`, { params });
  }
}
export const certificationsResolver: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_4_1_10_Certifications).getDropDownList();
};
