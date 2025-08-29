import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import { ContractManagementDto, ContractManagementParam, Contract_ManagementParamSource, Personal, PersonalParam, ProbationParam } from '@models/employee-maintenance/4_1_15_contract-management';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_4_1_15_ContractManagementService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl: string = environment.apiUrl + "C_4_1_15_ContractManagement/"
  initData: Contract_ManagementParamSource = <Contract_ManagementParamSource>{
    param: <ContractManagementParam>{
      effectiveStatus: 'Y'
    },
    currentPage: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    dataMain: []
  }
  programSource = signal<Contract_ManagementParamSource>(JSON.parse(JSON.stringify(this.initData)));
  programSource$ = toObservable(this.programSource);
  setSource = (source: Contract_ManagementParamSource) => this.programSource.set(source);
  clearParams = () => {
    this.programSource.set(JSON.parse(JSON.stringify(this.initData)))
  }

  constructor(private http: HttpClient) { }

  getData(pagination: PaginationParam, param: ContractManagementParam) {
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<ContractManagementDto>>(this.baseUrl + 'GetData', { params })
  }

  create(data: ContractManagementDto) {
    return this.http.post<OperationResult>(this.baseUrl + 'Create', data)
  }

  update(data: ContractManagementDto) {
    return this.http.put<OperationResult>(this.baseUrl + 'Update', data)
  }

  delete(data: ContractManagementDto) {
    return this.http.delete<OperationResult>(this.baseUrl + 'Delete', { params: {}, body: data });
  }

  getListDepartment(division: string, factory: string) {
    let params = new HttpParams().appendAll({ division, factory, lang: this.language })
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'GetListDepartment', { params });
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
  getListAssessmentResult() {
    let params = new HttpParams().appendAll({ lang: this.language })
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'getListAssessmentResult', { params });
  }

  getPerson(personal: PersonalParam) {
    personal.lang = this.language
    let params = new HttpParams().appendAll({ ...personal })
    return this.http.get<Personal>(this.baseUrl + 'GetPerson', { params });
  }

  getProbationDate(division: string, factory: string, contractType: string) {
    let params = new HttpParams().appendAll({ division, factory, contractType })
    return this.http.get<ProbationParam>(this.baseUrl + 'GetProbationDate', { params });
  }

  getEmployeeID() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'GetEmployeeID');
  }

}
