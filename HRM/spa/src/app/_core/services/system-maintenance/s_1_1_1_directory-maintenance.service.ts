import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { toObservable } from '@angular/core/rxjs-interop'
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { DirectoryMaintenance_Param, DirectoryMaintenance_Data, DirectoryMaintenance_Memory } from '@models/system-maintenance/1_1_1_directory-maintenance';
import { BehaviorSubject } from 'rxjs';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root'
})
export class S_1_1_1_DirectoryMaintenanceService implements IClearCache {
  apiUrl: string = environment.apiUrl + "C_1_1_1_DirectoryMaintenance/";
  initData: DirectoryMaintenance_Memory = <DirectoryMaintenance_Memory>{
    pagination: <Pagination>{ pageNumber: 1, pageSize: 10, totalCount: 0 },
    param: <DirectoryMaintenance_Param>{},
    selectedData: <DirectoryMaintenance_Data>{},
    data: []
  }
  directorySource = signal<DirectoryMaintenance_Memory>(structuredClone(this.initData))
  source = toObservable(this.directorySource);
  setSource = (source: DirectoryMaintenance_Memory) => this.directorySource.set(source);

  paramSearchSource = new BehaviorSubject<DirectoryMaintenance_Param>(null);
  currentParamSearch = this.paramSearchSource.asObservable();

  constructor(private http: HttpClient) { }

  clearParams() {
    this.directorySource.set(structuredClone(this.initData))
    this.paramSearchSource.next(null)
  }

  changeParamSearch(paramSearch: DirectoryMaintenance_Param) {
    this.paramSearchSource.next(paramSearch);
  }

  getData(pagination: PaginationParam, param: DirectoryMaintenance_Param) {
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<DirectoryMaintenance_Data>>(this.apiUrl + 'GetData', { params });
  }

  getParent() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetParentDirectoryCode');
  }

  deleteData(directoryCode: string) {
    let params = new HttpParams().appendAll({ directoryCode })
    return this.http.delete<OperationResult>(this.apiUrl + 'Delete', { params });
  }

  add(model: DirectoryMaintenance_Data) {
    return this.http.post<OperationResult>(this.apiUrl + 'Add', model)
  }

  edit(model: DirectoryMaintenance_Data) {
    return this.http.put<OperationResult>(this.apiUrl + 'Update', model)
  }
}
