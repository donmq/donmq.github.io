import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { HRMS_Emp_ResignationDto, ResignAddAndEditParam, ResignationManagementParam, ResignationManagementSource } from '@models/employee-maintenance/4_1_12_resignation-management';
import { Observable } from 'rxjs';
import { toObservable } from '@angular/core/rxjs-interop';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class S_4_1_12_ResignationManagementService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_4_1_12_ResignationManagement/`;
  initData: ResignationManagementSource = <ResignationManagementSource>{
    param: <ResignationManagementParam>{},
    startDate: null,
    endDate: null,
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    source: <HRMS_Emp_ResignationDto>{},
    data: []
  }
  paramSearch = signal<ResignationManagementSource>(JSON.parse(JSON.stringify(this.initData)));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: ResignationManagementSource) => this.paramSearch.set(data)
  clearParams = () => {
    this.paramSearch.set(JSON.parse(JSON.stringify(this.initData)))
  }

  constructor(private http: HttpClient) { }

  getData(pagination: PaginationParam, param: ResignationManagementParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<HRMS_Emp_ResignationDto>>(this.apiUrl + "GetData", { params })
  }

  getListDivision() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListDivision', { params: { language: this.language } }
    );
  }

  getListFactory(division: string) {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListFactory', { params: { division, language: this.language } }
    );
  }

  getListResignationType() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListResignationType', { params: { language: this.language } }
    );
  }

  getListResignReason(resignationType: string) {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListResignReason', { params: { language: this.language, resignationType } }
    );
  }

  getEmployeeID() {
    return this.http.get<string[]>(this.apiUrl + "GetEmployeeID", {});
  }

  getEmployeeData(factory: string, employeeID: string): Observable<HRMS_Emp_ResignationDto[]> {
    return this.http.get<HRMS_Emp_ResignationDto[]>(
      this.apiUrl + "GetEmployeeData", { params: { factory, employeeID } }
    );
  }

  getVerifierName(factory: string, verifier: string) {
    return this.http.get<OperationResult>(
      this.apiUrl + 'GetVerifierName', { params: { factory, verifier } }
    );
  }

  getVerifierTitle(factory: string, verifier: string) {
    return this.http.get<OperationResult>(
      this.apiUrl + 'GetVerifierTitle', { params: { factory, verifier, language: this.language } }
    );
  }

  addNew(param: ResignAddAndEditParam) {
    return this.http.post<OperationResult>(this.apiUrl + "AddNew", param);
  }

  edit(param: ResignAddAndEditParam) {
    return this.http.put<OperationResult>(this.apiUrl + "Edit", param)
  }

  delete(model: HRMS_Emp_ResignationDto) {
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { params: {}, body: model });
  }

  downloadExcel(param: ResignationManagementParam) {
    param.lang = this.language
    return this.http.get<OperationResult>(this.apiUrl + "DownloadExcel", { params: { ...param } });
  }
}
