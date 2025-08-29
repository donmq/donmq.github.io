import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { ResolveFn } from '@angular/router';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { CodeMaintenanceParam, HRMS_Basic_Code, HRMS_Basic_Code_Source } from '@models/basic-maintenance/2_1_4_code-maintenance';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class S_2_1_4_CodeMaintenanceService implements IClearCache {
  baseUrl = `${environment.apiUrl}C_2_1_4_CodeMaintenance`;
  initData: HRMS_Basic_Code_Source = <HRMS_Basic_Code_Source>{
    model: <HRMS_Basic_Code>{},
    data: [],
    param: <CodeMaintenanceParam>{},
    pagination: <Pagination>{ pageNumber: 1, pageSize: 10, totalCount: 0 }
  }
  basicCodeSource = signal<HRMS_Basic_Code_Source>(structuredClone(this.initData));
  source = toObservable(this.basicCodeSource);
  setSource = (source: HRMS_Basic_Code_Source) => this.basicCodeSource.set(source);
  clearParams = () => {
    this.basicCodeSource.set(structuredClone(this.initData))
  }

  constructor(private _http: HttpClient) { }

  getDataMainPagination(param: Pagination, filter: CodeMaintenanceParam): Observable<PaginationResult<HRMS_Basic_Code>> {
    let params = new HttpParams().appendAll({ ...param, ...filter });
    return this._http.get<PaginationResult<HRMS_Basic_Code>>(`${this.baseUrl}/GetDataPagination`, { params });
  }

  create(model: HRMS_Basic_Code) {
    return this._http.post<OperationResult>(`${this.baseUrl}/Create`, model);
  }

  update(model: HRMS_Basic_Code) {
    return this._http.put<OperationResult>(`${this.baseUrl}/Update`, model);
  }


  delete(typeSeq: string, code: string) {
    return this._http.delete<OperationResult>(`${this.baseUrl}/Delete`, { params: { typeSeq, code } });
  }

  download(filter: CodeMaintenanceParam) {
    let params = new HttpParams().appendAll({ ...filter });
    return this._http.get<OperationResult>(`${this.baseUrl}/Export`, { params });
  }

  getTypeSeqs() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}/GetTypeSeqs`);
  }
}

export const resolverTypeSeqs: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_2_1_4_CodeMaintenanceService).getTypeSeqs();
};
