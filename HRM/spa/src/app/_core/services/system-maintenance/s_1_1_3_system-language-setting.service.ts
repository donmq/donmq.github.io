import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@env/environment';
import {
  SystemLanguageSetting_Data,
  SystemLanguageSetting_Memory
} from '@models/system-maintenance/1_1_3-system-language-setting';
import {
  Pagination,
  PaginationResult,
} from '@utilities/pagination-utility';
import { OperationResult } from '@utilities/operation-result';
import { toObservable } from '@angular/core/rxjs-interop'
import { IClearCache } from '@services/cache.service';
@Injectable({
  providedIn: 'root'
})
export class S_1_1_3_SystemLanguageSettingService implements IClearCache {
  apiUrl: string = environment.apiUrl + "C_1_1_3_SystemLanguageSetting/";

  initData: SystemLanguageSetting_Memory = <SystemLanguageSetting_Memory>{
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <SystemLanguageSetting_Data>{}
  }
  basicCodeSource = signal<SystemLanguageSetting_Memory>(structuredClone(this.initData))
  source = toObservable(this.basicCodeSource);
  setSource = (source: SystemLanguageSetting_Memory) => this.basicCodeSource.set(source);
  clearParams = () => {
    this.basicCodeSource.set(structuredClone(this.initData))
  }
  constructor(private http: HttpClient) { }

  getAll(pagination: Pagination) {
    let params = new HttpParams().appendAll({ ...pagination });
    return this.http.get<PaginationResult<SystemLanguageSetting_Data>>(this.apiUrl + 'GetData', { params })
  }

  create(model: SystemLanguageSetting_Data) {
    return this.http.post<OperationResult>(this.apiUrl + 'Create', model);
  }

  update(model: SystemLanguageSetting_Data) {
    return this.http.put<OperationResult>(this.apiUrl + 'Update', model);
  }
}
