import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal, } from '@angular/core';
import { environment } from '@env/environment';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { KeyValuePair } from '@utilities/key-value-pair';
import { HRMS_Basic_Factory_Comparison, HRMS_Basic_Factory_ComparisonSource } from '@models/basic-maintenance/2_1_7_factory-comparison';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_2_1_7_FactoryComparisonService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) };
  apiUrl: string = environment.apiUrl + "C_2_1_7_FactoryComparison";
  initData: HRMS_Basic_Factory_ComparisonSource = <HRMS_Basic_Factory_ComparisonSource>{
    pagination: <Pagination>{ pageNumber: 1, pageSize: 10, totalCount: 0 },
    data: []
  }
  factoryComparisonSource = signal<HRMS_Basic_Factory_ComparisonSource>(structuredClone(this.initData));
  factoryComparisonSource$ = toObservable(this.factoryComparisonSource);
  setSource = (source: HRMS_Basic_Factory_ComparisonSource) => this.factoryComparisonSource.set(source);
  clearParams = () => {
    this.factoryComparisonSource.set(structuredClone(this.initData))
  }

  constructor(private _http: HttpClient) { }

  getDivisions() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/GetDivisions`);
  }

  getFactories() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/GetFactories`);
  }

  getDataMainPagination(param: Pagination, kind: string) {
    let params = new HttpParams().appendAll({ ...param, kind });
    return this._http.get<PaginationResult<HRMS_Basic_Factory_Comparison>>(`${this.apiUrl}/GetData`, { params });
  }

  create(model: HRMS_Basic_Factory_Comparison[]) {
    return this._http.post<OperationResult>(`${this.apiUrl}/Create`, model);
  }

  delete(model: HRMS_Basic_Factory_Comparison) {
    return this._http.delete<OperationResult>(`${this.apiUrl}/Delete`, { params: {}, body: model });
  }
}
