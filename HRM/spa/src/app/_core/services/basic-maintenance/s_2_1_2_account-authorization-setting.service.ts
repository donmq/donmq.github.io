import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@env/environment';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import {
  AccountAuthorizationSetting_Param,
  AccountAuthorizationSetting_Memory,
  AccountAuthorizationSetting_Data
} from '@models/basic-maintenance/2_1_2_account-authorization-setting';
import { OperationResult } from '@utilities/operation-result';
import { toObservable } from '@angular/core/rxjs-interop'
import { KeyValuePair } from '@utilities/key-value-pair';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';
@Injectable({
  providedIn: 'root'
})
export class S_2_1_2_AccountAuthorizationSettingService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) };
  apiUrl: string = environment.apiUrl + "C_2_1_2_AccountAuthorizationSetting/";

  initData: AccountAuthorizationSetting_Memory = <AccountAuthorizationSetting_Memory>{
    param: <AccountAuthorizationSetting_Param>{
      isActive: 1,
      listRole: []
    },
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <AccountAuthorizationSetting_Data>{},
    data: []
  }
  paramSearch = signal<AccountAuthorizationSetting_Memory>(structuredClone(this.initData))
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: AccountAuthorizationSetting_Memory) => this.paramSearch.set(data)

  constructor(private http: HttpClient) { }

  clearParams() {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getData(pagination: PaginationParam, param: AccountAuthorizationSetting_Param) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<AccountAuthorizationSetting_Data>>(this.apiUrl + 'GetData', { params })
  }

  getListDepartment(division: string, factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params: { division, factory, language : this.language } });
  }

  getListDivision() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDivision', { params: { language : this.language  } });
  }

  getListListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language : this.language  } });
  }

  getListRole() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListRole');
  }

  create(param: AccountAuthorizationSetting_Data) {
    return this.http.post<OperationResult>(this.apiUrl + "Create", param);
  }

  update(param: AccountAuthorizationSetting_Data) {
    return this.http.put<OperationResult>(this.apiUrl + "Update", param);
  }
  resetPassword(param: AccountAuthorizationSetting_Data) {
    return this.http.put<OperationResult>(this.apiUrl + "ResetPassword", param);
  }
  download(param: AccountAuthorizationSetting_Param) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadExcel', { params })
  }
}
