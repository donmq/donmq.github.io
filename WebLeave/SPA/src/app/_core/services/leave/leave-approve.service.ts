import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '@env/environment';
import { SearchApproveParams } from '@params/leave/leave-approve-params';
import { LeaveDataApprove } from '@models/leave/leave-data-approve';
import { KeyValuePair } from '@utilities/key-value-pair';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class LeaveApproveService {
  apiUrl = environment.apiUrl;
  dataSource = new BehaviorSubject<SearchApproveParams>(null)
  currentDataSource = this.dataSource.asObservable();
  constructor(private http: HttpClient) { }
  //call API
  getCategory(lang: string) {
    let params = new HttpParams()
      .set('lang', lang);
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}LeaveApprove/GetCategory`, {params});
  }
  getLeaveData(paramsSearch: SearchApproveParams, pagination: Pagination)
    : Observable<PaginationResult<LeaveDataApprove>> {
    let paramsTmp = {...paramsSearch, ...pagination};
    let params = new HttpParams().appendAll(paramsTmp);
    return this.http.get<PaginationResult<LeaveDataApprove>>(this.apiUrl + 'LeaveApprove/GetLeaveData', { params });
  }
  updateLeaveData(models: LeaveDataApprove[], checkUpdate: Boolean) {
    let params = new HttpParams()
      .set('checkUpdate', checkUpdate.valueOf());
    return this.http.put<OperationResult>(this.apiUrl + 'LeaveApprove/UpdateLeaveData', models, {params});
  }
  exportExcel(pagination: Pagination, param: SearchApproveParams) {
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<OperationResult>(this.apiUrl + "LeaveApprove/ExportExcel", { params })
  }
}
