import {
  HRMS_Emp_IDcard_EmpID_HistoryDto,
  IdentificationCardToEmployeeIDHistoryParam,
  IdentificationCardToEmployeeIDHistorySource
} from '@models/employee-maintenance/4_1_7_identification-card-to-employee-id-history';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@env/environment';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class S_4_1_7_IdentificationCardToEmployeeIdHistoryService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_4_1_7_IdentificationCardToEmployeeIDHistory/`;
  initData: IdentificationCardToEmployeeIDHistorySource = <IdentificationCardToEmployeeIDHistorySource>{
    param: <IdentificationCardToEmployeeIDHistoryParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<IdentificationCardToEmployeeIDHistorySource>(JSON.parse(JSON.stringify(this.initData)));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: IdentificationCardToEmployeeIDHistorySource) => this.paramSearch.set(data)

  constructor(private http: HttpClient) { }

  clearParams = () => {
    this.paramSearch.set(JSON.parse(JSON.stringify(this.initData)))
  }

  getData(pagination: PaginationParam, param: IdentificationCardToEmployeeIDHistoryParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<HRMS_Emp_IDcard_EmpID_HistoryDto>>(this.apiUrl + "GetData", { params })
  }

  getListDivision() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListDivision', { params: { language: this.language } }
    );
  }

  getListFactory(division: string,) {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListFactory', { params: { division, language: this.language } }
    );
  }

  getListNationality() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListNationality', { params: { language: this.language } }
    );
  }
}
