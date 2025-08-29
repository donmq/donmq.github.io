import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@env/environment';
import {
  EmployeeTransferHistory,
  EmployeeTransferHistoryDTO,
  EmployeeTransferHistoryDetele,
  EmployeeTransferHistoryEffectiveConfirm,
  EmployeeTransferHistoryParam,
  EmployeeTransferHistoryDetail,
  EmployeeTransferHistorySource
} from '@models/employee-maintenance/4_1_17_employee-transfer-history';
import { toObservable } from '@angular/core/rxjs-interop';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { BehaviorSubject } from 'rxjs';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LocalStorageConstants } from '@constants/local-storage.constants';
@Injectable({
  providedIn: 'root'
})
export class S_4_1_17_EmployeeTransferHistoryService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + "C_4_1_17_EmployeeTransferHistory/"

  initData: EmployeeTransferHistory = <EmployeeTransferHistory>{
    param: <EmployeeTransferHistoryParam>{
      effective_Status: 0
    },
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<EmployeeTransferHistory>(JSON.parse(JSON.stringify(this.initData)));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: EmployeeTransferHistory) => this.paramSearch.set(data)

  initDataCodeSource: EmployeeTransferHistorySource = <EmployeeTransferHistorySource>{
    currentPage: 1,
    param: <EmployeeTransferHistoryParam>{},
    basicCode: <EmployeeTransferHistoryDTO>{}
  }
  basicCodeSource = signal<EmployeeTransferHistorySource>(JSON.parse(JSON.stringify(this.initDataCodeSource)));
  basicCodeSource$ = toObservable(this.basicCodeSource);
  setSource = (source: EmployeeTransferHistorySource) => this.basicCodeSource.set(source);

  initDataMethod: string = null
  method = signal<string>(JSON.parse(JSON.stringify(this.initDataMethod)));
  method$ = toObservable(this.method);
  setMethod = (source: string) => this.method.set(source);

  paramSearchSource = new BehaviorSubject<EmployeeTransferHistoryParam>(null);
  paramSearchSource$ = this.paramSearchSource.asObservable();
  changeParamSearch(paramsearch: EmployeeTransferHistoryParam) { this.paramSearchSource.next(paramsearch); }

  constructor(private http: HttpClient) { }

  clearParams() {
    this.paramSearch.set(JSON.parse(JSON.stringify(this.initData)))
    this.basicCodeSource.set(JSON.parse(JSON.stringify(this.initDataCodeSource)))
    this.method.set(JSON.parse(JSON.stringify(this.initDataMethod)))
    this.paramSearchSource.next(null)
  }

  getData(pagination: PaginationParam, param: EmployeeTransferHistoryParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<EmployeeTransferHistoryDetail>>(this.apiUrl + 'GetData', { params })
  }

  download(param: EmployeeTransferHistoryParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadExcel', { params })
  }

  create(param: EmployeeTransferHistoryDTO) {
    return this.http.post<OperationResult>(this.apiUrl + "Create", param);
  }

  update(param: EmployeeTransferHistoryDTO) {
    return this.http.put<OperationResult>(this.apiUrl + "Update", param);
  }
  delete(param: EmployeeTransferHistoryDetele) {
    let params = new HttpParams().appendAll({ ...param });
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { params });
  }
  batchDelete(params: EmployeeTransferHistoryDetele[]) {
    return this.http.delete<OperationResult>(this.apiUrl + "BatchDelete", { body: params });
  }
  effectiveConfirm(params: EmployeeTransferHistoryEffectiveConfirm[]) {
    return this.http.put<OperationResult>(this.apiUrl + "EffectiveConfirm", params);
  }
  checkEffectiveConfirm(data: EmployeeTransferHistoryEffectiveConfirm) {
    let params = new HttpParams().appendAll({ ...data });
    return this.http.get<OperationResult>(this.apiUrl + 'CheckEffectiveConfirm', { params });
  }
  getListDivision() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListDivision', { params: { language: this.language } }
    );
  }

  getListFactory(division: string) {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListFactory', { params: { language: this.language, division } }
    );
  }

  getListDepartment(factory: string, division: string) {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListDepartment', { params: { language: this.language, factory, division } }
    );
  }

  getListAssignedDivisionAfter() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'getListAssignedDivisionAfter', { params: { language: this.language } }
    );
  }

  getListWorkType() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListWorkType', { params: { language: this.language } }
    );
  }

  getListAssignedFactoryAfter(assignedDivisionAfter: string) {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListAssignedFactoryAfter', { params: { language: this.language, assignedDivisionAfter } }
    );
  }

  getListDepartmentAfter(assignedFactoryAfter: string, assignedDivisionAfter: string) {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListDepartmentAfter', { params: { language: this.language, assignedFactoryAfter, assignedDivisionAfter } }
    );
  }
  getListPositionGrade() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListPositionGrade', { params: { language: this.language } });
  }

  getListPositionTitle(positionGrade: number) {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListPositionTitle', { params: { language: this.language, positionGrade } }
    );
  }

  getDataDetail(division: string, employee_ID: string, factory: string) {
    return this.http.get<EmployeeTransferHistoryDTO>(
      this.apiUrl + 'GetDataDetail', { params: { division, employee_ID, factory } }
    )
  }

  getListReasonforChange() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListReasonforChange', { params: { language: this.language } }
    );
  }
  getListTypeHeadEmployeeID(factory: string, division: string) {
    return this.http.get<string[]>(
      this.apiUrl + 'GetListTypeHeadEmployeeID', { params: { factory, division } }
    );
  }
  getListDataSource() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListDataSource', { params: { language: this.language } }
    );
  }
}
