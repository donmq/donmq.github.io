import { HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import { HttpClient } from '@angular/common/http';
import { MonthlySalaryMasterFileBackupQueryDto, MonthlySalaryMasterFileBackupQueryParam, SalaryDetailDto, MonthlySalaryMasterFileBackupQuerySource } from '@models/salary-maintenance/7_1_17_monthly-salary-master-file-backup-query';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { OperationResult } from '@utilities/operation-result';
import { Observable } from 'rxjs';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_17_MonthlySalaryMasterFileBackupQueryService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }

  constructor(private http: HttpClient) { }
  apiUrl = `${environment.apiUrl}C_7_1_17_MonthlySalaryMasterFileBackupQuery/`;

  initData = <MonthlySalaryMasterFileBackupQuerySource>{
    salarySearch_Param: <MonthlySalaryMasterFileBackupQueryParam>{},
    batchData_Param: <MonthlySalaryMasterFileBackupQueryParam>{},
    salarySearch_Data: [],
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <MonthlySalaryMasterFileBackupQueryDto>{},
    selected_Tab: 'salarySearch'
  }
  paramSearch = signal<MonthlySalaryMasterFileBackupQuerySource>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: MonthlySalaryMasterFileBackupQuerySource) => this.paramSearch.set(data)

  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getData(pagination: PaginationParam, param: MonthlySalaryMasterFileBackupQueryParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<OperationResult>(this.apiUrl + "GetData", { params })
  }

  getSalaryDetails(pagination: PaginationParam, probation: string, factory: string, employeeID: string, yearMonth: string) {
    return this.http.get<PaginationResult<SalaryDetailDto>>(this.apiUrl + 'GetSalaryDetails', { params: { ...pagination, probation, factory, employeeID, language: this.language, yearMonth } });
  }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }

  getListDepartment(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params: { factory, language: this.language } });
  }

  getListPositionTitle() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListPositionTitle', { params: { language: this.language } });
  }

  getListPermissionGroup() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListPermissionGroup', { params: { language: this.language } });
  }

  getListSalaryType() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListSalaryType', { params: { language: this.language } });
  }

  getListTechnicalType() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListTechnicalType', { params: { language: this.language } });
  }

  getListExpertiseCategory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListExpertiseCategory', { params: { language: this.language } });
  }

  execute(data: MonthlySalaryMasterFileBackupQueryParam): Observable<OperationResult> {
    data.lang = this.language
    return this.http.post<OperationResult>(this.apiUrl + 'Execute', data);
  }

  download(param: MonthlySalaryMasterFileBackupQueryParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadFileExcel', { params })
  }
}
