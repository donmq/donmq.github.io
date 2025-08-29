import { OperationResult } from '@utilities/operation-result';
import { HRMS_Emp_Blacklist_MainMemory, HRMS_Emp_BlacklistDto, HRMS_Emp_BlacklistParam } from '@models/employee-maintenance/4_1_13_exit-employees-blacklist';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_4_1_13_ExitEmployeesBlacklistService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_4_1_13_ExitEmployeesBlacklist/`;
  initData: HRMS_Emp_Blacklist_MainMemory = <HRMS_Emp_Blacklist_MainMemory>{
    param: <HRMS_Emp_BlacklistParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<HRMS_Emp_Blacklist_MainMemory>(JSON.parse(JSON.stringify(this.initData)));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: HRMS_Emp_Blacklist_MainMemory) => this.paramSearch.set(data)
  clearParams = () => {
    this.paramSearch.set(JSON.parse(JSON.stringify(this.initData)))
  }

  constructor(private http: HttpClient) { }

  getData(pagination: PaginationParam, param: HRMS_Emp_BlacklistParam) {
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<HRMS_Emp_BlacklistDto>>(this.baseUrl + 'GetData', { params })
  }

  getListNationality() {
    return this.http.get<KeyValuePair[]>(
      this.baseUrl + 'GetListNationality', { params: { language: this.language } }
    );
  }

  getIdentificationNumber() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'GetListIdentificationNumber');
  }

  edit(model: HRMS_Emp_BlacklistDto) {
    return this.http.put<OperationResult>(this.baseUrl + 'Update', model)
  }

}
