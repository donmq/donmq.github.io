import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { ResolveFn } from '@angular/router';
import {
  DocumentManagement_SubModel,
  DocumentManagement_MainData,
  DocumentManagement_MainMemory,
  DocumentManagement_MainParam,
  DocumentManagement_SubMemory,
  DocumentManagement_SubParam,
  DocumentManagement_DownloadFileModel,
  DocumentManagement_TypeheadKeyValue
} from '@models/employee-maintenance/4_1_9_document-management';
import { toObservable } from '@angular/core/rxjs-interop';
import { BehaviorSubject, Observable } from 'rxjs';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { environment } from '@env/environment';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root',
})

export class S_4_1_9_DocumentManagement implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_4_1_9_DocumentManagement/`;
  initData: DocumentManagement_MainMemory = <DocumentManagement_MainMemory>{
    param: <DocumentManagement_MainParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<DocumentManagement_MainMemory>(JSON.parse(JSON.stringify(this.initData)));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: DocumentManagement_MainMemory) => this.paramSearch.set(data)

  paramForm = new BehaviorSubject<DocumentManagement_SubParam>(null);
  paramForm$ = this.paramForm.asObservable();
  setParamForm = (item: DocumentManagement_SubParam) => this.paramForm.next(item);

  constructor(private http: HttpClient) { }

  clearParams() {
    this.paramSearch.set(JSON.parse(JSON.stringify(this.initData)))
    this.paramForm.next(null)
  }
  getDropDownList(division?: string) {
    let param: DocumentManagement_MainParam = <DocumentManagement_MainParam>{ lang: this.language }
    if (division)
      param.division = division
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  getEmployeeList(division: string, factory: string, employee_Id: string) {
    let param: DocumentManagement_SubParam = <DocumentManagement_SubParam>{
      division: division,
      factory: factory,
      employee_Id: employee_Id
    }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<DocumentManagement_TypeheadKeyValue[]>(` ${this.baseUrl}GetEmployeeList`, { params });
  }
  getSearchDetail(param: Pagination, filter: DocumentManagement_MainParam): Observable<PaginationResult<DocumentManagement_MainData>> {
    filter.lang = this.language
    let params = new HttpParams().appendAll({ ...param, ...filter });
    return this.http.get<PaginationResult<DocumentManagement_MainData>>(
      `${this.baseUrl}GetSearchDetail`, { params }
    );
  }
  getSubDetail(param: DocumentManagement_SubParam) {
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(` ${this.baseUrl}GetSubDetail`, { params });
  }
  checkExistedData(data: DocumentManagement_SubModel) {
    let params = new HttpParams().appendAll({ ...data });
    return this.http.get<OperationResult>(` ${this.baseUrl}CheckExistedData`, { params });
  }
  downloadFile(data: DocumentManagement_DownloadFileModel): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.baseUrl}DownloadFile`, data);
  }
  putData(data: DocumentManagement_SubMemory): Observable<OperationResult> {
    return this.http.put<OperationResult>(`${this.baseUrl}PutData`, data);
  }
  postData(data: DocumentManagement_SubMemory): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.baseUrl}PostData`, data);
  }
  deleteData(data: DocumentManagement_SubModel) {
    let params = new HttpParams().appendAll({ ...data })
    return this.http.delete<OperationResult>(`${this.baseUrl}DeleteData`, { params });
  }
  downloadExcel(param: DocumentManagement_MainParam) {
    param.lang = localStorage.getItem(LocalStorageConstants.LANG)
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(` ${this.baseUrl}DownloadExcel`, { params });
  }
}
export const documentManagementResolver: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_4_1_9_DocumentManagement).getDropDownList();
};
