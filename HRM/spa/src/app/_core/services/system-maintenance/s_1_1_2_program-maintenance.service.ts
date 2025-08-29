import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { toObservable } from '@angular/core/rxjs-interop'
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import {
  ProgramMaintenance_Param,
  ProgramMaintenance_Data,
  ProgramMaintenance_Memory
} from '@models/system-maintenance/1_1_2-program-maintenance';
import { OperationResult } from '@utilities/operation-result';
import { KeyValuePair } from '@utilities/key-value-pair';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root'
})
export class S_1_1_2_ProgramMaintenanceService implements IClearCache {
  apiUrl: string = `${environment.apiUrl}C_1_1_2_ProgramMaintenance/`;
  initData: ProgramMaintenance_Memory = <ProgramMaintenance_Memory>{
    param: <ProgramMaintenance_Param>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <ProgramMaintenance_Data>{},
    data: []
  }
  param = signal<ProgramMaintenance_Memory>(structuredClone(this.initData))
  source = toObservable(this.param);
  setSource = (source: ProgramMaintenance_Memory) => this.param.set(source);
  clearParams = () => {
    this.param.set(structuredClone(this.initData))
  }
  constructor(private http: HttpClient) { }

  getData(pagination: Pagination, param: ProgramMaintenance_Param) {
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<ProgramMaintenance_Data>>(this.apiUrl + "getData", { params: params });
  }

  getDirectory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + "getDirectory");
  }
  getFunction_ALL() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + "getFunction_ALL");
  }
  addNew(params: ProgramMaintenance_Data) {
    return this.http.post<OperationResult>(this.apiUrl + "add", params);
  }
  edit(params: ProgramMaintenance_Data) {
    return this.http.put<OperationResult>(this.apiUrl + "edit", params);
  }
  delete(program_Code: string) {
    let params = new HttpParams().set('Program_Code', program_Code)
    return this.http.delete<OperationResult>(this.apiUrl + "delete", { params: params });
  }

}

