import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { PermissionParam, PermissionRightsDTO } from '@models/seahr/permissionRightsDTO';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SeahrPermissionRightsService {
  private baseUrl: string = environment.apiUrl + "PermissionRights/";
  dataSource = new BehaviorSubject<PermissionParam>(null)
  currentDataSource = this.dataSource.asObservable();

  constructor(private http: HttpClient) { }
  getDataPagination(pagination: PaginationParam, param: PermissionParam) {
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<PermissionRightsDTO>>(this.baseUrl + 'GetData', { params });
  }

  getPart() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListParts");
  }

  exportExcel(param: PermissionParam) {
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.baseUrl + "ExportExcel", { params })
  }
}
