import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@env/environment';
import { SeaConfirmParam } from '@params/seahr/sea-confirm.param';
import { Pagination } from '@utilities/pagination-utility';
import { BehaviorSubject, Observable } from 'rxjs';
import { KeyValueUtility } from '@utilities/key-value-utility';
import { SeaConfirmEmpDetail } from '@models/seahr/sea-confirm-emp-detail';
import { SeaConfirmSearch } from '@models/seahr/sea-confirm';
import { OperationResult } from '@utilities/operation-result';
import { LeaveData } from '@models/common/leave-data';

@Injectable({ providedIn: 'root' })
export class SeaConfirmService {
  private apiUrl: string = environment.apiUrl;
  dataSource = new BehaviorSubject<SeaConfirmParam>(null)
  currentDataSource = this.dataSource.asObservable();

  constructor(private http: HttpClient) { }

  search(param: SeaConfirmParam, pagination: Pagination): Observable<SeaConfirmSearch> {
    let params = new HttpParams().appendAll({ ...this.getParams({ ...param }), ...pagination });
    return this.http.get<SeaConfirmSearch>(this.apiUrl + 'SeaConfirm/Search', { params });
  }

  getCategories(): Observable<KeyValueUtility[]> {
    return this.http.get<KeyValueUtility[]>(this.apiUrl + 'SeaConfirm/Categories');
  }

  getDepartments(): Observable<KeyValueUtility[]> {
    return this.http.get<KeyValueUtility[]>(this.apiUrl + 'SeaConfirm/Departments');
  }

  getParts(deptID: number): Observable<KeyValueUtility[]> {
    let params = new HttpParams().appendAll({ deptID });
    return this.http.get<KeyValueUtility[]>(this.apiUrl + 'SeaConfirm/Parts', { params });
  }

  getEmpDetail(empID: number): Observable<SeaConfirmEmpDetail> {
    let params = new HttpParams().appendAll({ empID });
    return this.http.get<SeaConfirmEmpDetail>(this.apiUrl + 'SeaConfirm/EmpDetail', { params });
  }

  getLeaveDeleteTopFive(empID: number): Observable<SeaConfirmEmpDetail> {
    let params = new HttpParams().appendAll({ empID });
    return this.http.get<SeaConfirmEmpDetail>(this.apiUrl + 'SeaConfirm/GetLeaveDeleteTopFive', { params });
  }

  confirm(body: LeaveData[]): Observable<OperationResult> {
    return this.http.put<OperationResult>(this.apiUrl + 'SeaConfirm/Confirm', body);
  }

  exportExcel(param: SeaConfirmParam, pagination: Pagination) {
    let params = new HttpParams().appendAll({...param, ...pagination});
    return this.http.get<OperationResult>(this.apiUrl + 'SeaConfirm/ExportExcel', { params })
  }

  private getParams(param: SeaConfirmParam) {
    const _param = { ...param };
    _param.cateID = _param.cateID ?? -1;
    _param.partID = _param.partID ?? -1;
    _param.deptID = _param.deptID ?? -1;
    _param.fromDate = _param.fromDate ?? '';
    _param.toDate = _param.toDate ?? '';
    return _param;
  }
}
