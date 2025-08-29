import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import { ContractManagementReportDto, ContractManagementReportParam, Contract_Management_ReportParamSource } from '@models/employee-maintenance/4_1_16_contract-management-report';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_4_1_16_ContractManagementReportService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl: string = environment.apiUrl + "C_4_1_16_ContractManagementReport/"

  initData: Contract_Management_ReportParamSource = <Contract_Management_ReportParamSource>{
    param: <ContractManagementReportParam>{ document_Type: '1' },
    currentPage: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    dataMain: []
  }
  programSource = signal<Contract_Management_ReportParamSource>(JSON.parse(JSON.stringify(this.initData)));
  programSource$ = toObservable(this.programSource);
  setSource = (source: Contract_Management_ReportParamSource) => this.programSource.set(source);
  clearParams = () => {
    this.programSource.set(JSON.parse(JSON.stringify(this.initData)))
  }

  constructor(private http: HttpClient) { }

  getData(pagination: PaginationParam, param: ContractManagementReportParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<ContractManagementReportDto>>(this.baseUrl + 'GetData', { params })
  }

  downloadExcel(param: ContractManagementReportParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(`${this.baseUrl}DownloadExcel`, { params });
  }

  getListDivision() {
    let params = new HttpParams().appendAll({ lang: this.language })
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'GetListDivision', { params });
  }
  getListFactory(division: string) {
    let params = new HttpParams().appendAll({ division, lang: this.language })
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'GetListFactory', { params });
  }
  getListContractType(division: string, factory: string) {
    let params = new HttpParams().appendAll({ division, factory, lang: this.language })
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'GetListContractType', { params });
  }
  getListDepartment(division: string, factory: string) {
    let params = new HttpParams().appendAll({ division, factory, lang: this.language })
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'GetListDepartment', { params });
  }

}
