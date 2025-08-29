import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import {
  AdditionDeductionItemToAccountingCodeMappingMaintenanceDto,
  AdditionDeductionItemToAccountingCodeMappingMaintenanceMemory,
  AdditionDeductionItemToAccountingCodeMappingMaintenanceParam
} from "@models/salary-maintenance/7_1_11_addition-deduction-item-to-accouting-code-mapping-maintenance";
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root',
})
export class S_7_1_11_AdditionDeductionItemToAccountingCodeMappingMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_7_1_11_AdditionDeductionItemToAccountingCodeMappingMaintenance/`;

  initData: AdditionDeductionItemToAccountingCodeMappingMaintenanceMemory = <AdditionDeductionItemToAccountingCodeMappingMaintenanceMemory>{
    param: <AdditionDeductionItemToAccountingCodeMappingMaintenanceParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0,
      totalPage: 0
    },
    data: [],
    selectedData: <AdditionDeductionItemToAccountingCodeMappingMaintenanceDto>{}
  }
  paramSearch = signal<AdditionDeductionItemToAccountingCodeMappingMaintenanceMemory>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: AdditionDeductionItemToAccountingCodeMappingMaintenanceMemory) => this.paramSearch.set(data)

  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  constructor(private _http: HttpClient) { }

  getDataPagination(pagination: Pagination, param: AdditionDeductionItemToAccountingCodeMappingMaintenanceParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param })
    return this._http.get<PaginationResult<AdditionDeductionItemToAccountingCodeMappingMaintenanceDto>>(`${this.baseUrl}GetDataPagination`, { params })
  }

  create(data: AdditionDeductionItemToAccountingCodeMappingMaintenanceDto) {
    return this._http.post<OperationResult>(`${this.baseUrl}Create`, data);
  }

  update(data: AdditionDeductionItemToAccountingCodeMappingMaintenanceDto) {
    return this._http.put<OperationResult>(`${this.baseUrl}Update`, data);
  }

  delete(data: AdditionDeductionItemToAccountingCodeMappingMaintenanceDto) {
    return this._http.delete<OperationResult>(`${this.baseUrl}Delete`, {
      body: data,
    });
  }

  getListFactoryByUser() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetListFactoryByUser`, {
      params: { language : this.language },
    });
  }

  getListAdditionsAndDeductionsItem() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetListAdditionsAndDeductionsItem`, {
      params: { language: this.language  },
    });
  }
}
