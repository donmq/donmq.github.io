import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import {
  EmployeeTransferOperationInboundDto,
  EmployeeTransferOperationInboundMainMemory,
  EmployeeTransferOperationInboundParam
} from '@models/employee-maintenance/4_1_21_employee-transfer-operation-inbound';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import {
  Pagination,
  PaginationParam,
  PaginationResult,
} from '@utilities/pagination-utility';
import { BehaviorSubject } from 'rxjs';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root',
})
export class S_4_1_21_EmployeeTransferOperationInboundService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl: string = environment.apiUrl + 'C_4_1_21_EmployeeTransferOperationInbound/';
  initData: EmployeeTransferOperationInboundMainMemory = <EmployeeTransferOperationInboundMainMemory>{
    param: <EmployeeTransferOperationInboundParam>{ effectiveStatus: '' },
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0,
    },
    data: [],
  };

  //#region Set data với Signal
  paramSearch = signal<EmployeeTransferOperationInboundMainMemory>(JSON.parse(JSON.stringify(this.initData)));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: EmployeeTransferOperationInboundMainMemory) => this.paramSearch.set(data);

  paramForm = new BehaviorSubject<string>(null);
  paramForm$ = this.paramForm.asObservable();
  setParamForm = (item: string) => this.paramForm.next(item);
  //#endregion

  constructor(private http: HttpClient) { }
  clearParams() {
    this.paramSearch.set(JSON.parse(JSON.stringify(this.initData)))
    this.paramForm.next(null);
  }

  edit(dto: EmployeeTransferOperationInboundDto) {
    return this.http.put<OperationResult>(`${this.baseUrl}Edit`, dto);
  }

  confirm(dto: EmployeeTransferOperationInboundDto) {
    return this.http.put<OperationResult>(`${this.baseUrl}Confirm`, dto);
  }

  getPagination(pagination: PaginationParam, param: EmployeeTransferOperationInboundParam) {
    param.language = this.language
    var params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<
      PaginationResult<EmployeeTransferOperationInboundDto>
    >(`${this.baseUrl}GetPagination`, {
      params,
    });
  }

  getDetail(history_GUID: string) {
    var params = new HttpParams().appendAll({
      history_GUID,
    });
    return this.http.get<EmployeeTransferOperationInboundDto>(
      `${this.baseUrl}GetDetail`,
      {
        params,
      }
    );
  }

  //#region Get List
  getListDivision() {
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetListDivision`, {
      params: { language: this.language },
    });
  }
  getListFactory() {
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetListFactory`, {
      params: { language: this.language },
    });
  }
  getListFactoryByDivision(division: string) {
    var params = new HttpParams().appendAll({ division, language: this.language });
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListFactoryByDivision`, { params }
    );
  }
  getListNationality() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListNationality`, { params: { language: this.language } }
    );
  }
  getListWorkType() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListWorkType`, { params: { language: this.language } }
    );
  }

  getListReasonChangeOut() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListReasonChangeOut`, { params: { language: this.language } }
    );
  }

  getListReasonChangeIn() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListReasonChangeIn`, { params: { language: this.language } }
    );
  }

  getListDepartment(division: string, factory: string) {
    var params = new HttpParams().appendAll({ division, factory, language: this.language });
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListDepartment`, { params }
    );
  }

  getListPositionGrade() {
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetPositionGrade`);
  }

  getListPositionTitle(level: number) {
    var params = new HttpParams().appendAll({ level, language: this.language });
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetPositionTitle`, { params }
    );
  }
  //#endregion
}
