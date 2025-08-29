import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import { ContractTypeSetup_MainMemory, ContractTypeSetupDto, ContractTypeSetupParam, ContractTypeSetupSource } from '@models/employee-maintenance/4_1_14_contract-type-setup';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_4_1_14_ContractTypeSetupService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_4_1_14_ContractTypeSetup/`;
  initData: ContractTypeSetup_MainMemory = <ContractTypeSetup_MainMemory>{
    param: <ContractTypeSetupParam>{
      probationary_Period_Str: '',
      alert_Str: ''
    },
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<ContractTypeSetup_MainMemory>(JSON.parse(JSON.stringify(this.initData)));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: ContractTypeSetup_MainMemory) => this.paramSearch.set(data)

  initDataSource: ContractTypeSetupSource = <ContractTypeSetupSource>{
    paramQuery: <ContractTypeSetupParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
  }
  contractTypeSetupSource = signal<ContractTypeSetupSource>(JSON.parse(JSON.stringify(this.initDataSource)));
  contractTypeSetupSource$ = toObservable(this.contractTypeSetupSource);
  setSource = (source: ContractTypeSetupSource) => this.contractTypeSetupSource.set(source);

  constructor(private _http: HttpClient) { }

  clearParams() {
    this.paramSearch.set(JSON.parse(JSON.stringify(this.initData)))
    this.contractTypeSetupSource.set(JSON.parse(JSON.stringify(this.initDataSource)))
  }

  getData(pagination: PaginationParam, param: ContractTypeSetupParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this._http.get<PaginationResult<ContractTypeSetupDto>>(this.baseUrl + 'GetData', { params })
  }

  getDataDetail(param: ContractTypeSetupParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this._http.get<ContractTypeSetupDto[]>(this.baseUrl + 'GetDataDetail', { params })
  }

  add(model: ContractTypeSetupDto) {
    return this._http.post<OperationResult>(this.baseUrl + 'Create', model);
  }

  edit(model: ContractTypeSetupDto) {
    return this._http.put<OperationResult>(this.baseUrl + 'Update', model);
  }

  delete(model: ContractTypeSetupParam) {
    model.lang = this.language
    return this._http.delete<OperationResult>(
      this.baseUrl + 'Delete', { params: {}, body: model }
    );
  }

  downloadExcel(param: ContractTypeSetupParam) {
    param.lang = this.language
    return this._http.get<OperationResult>(
      this.baseUrl + "DownloadExcel", { params: { ...param } }
    );
  }

  getListDivision() {
    return this._http.get<KeyValuePair[]>(
      this.baseUrl + 'GetListDivision', { params: { language: this.language } }
    );
  }

  getListFactory(division: string) {
    return this._http.get<KeyValuePair[]>(
      this.baseUrl + 'GetListFactory', { params: { division, language: this.language } }
    );
  }

  getListScheduleFrequency() {
    return this._http.get<KeyValuePair[]>(
      this.baseUrl + 'GetListScheduleFrequency', { params: { language: this.language } }
    );
  }

  getListContractType(division: string, factory: string) {
    var params = new HttpParams().appendAll({ division, factory, language: this.language }

    );
    return this._http.get<KeyValuePair[]>(
      this.baseUrl + 'GetListContractType', { params });
  }

  getListAlertRule() {
    return this._http.get<KeyValuePair[]>(
      this.baseUrl + 'GetListAlertRule', { params: { language: this.language } }
    );
  }
}
