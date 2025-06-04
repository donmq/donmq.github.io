import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { AllowLeaveSundayDto, AllowLeaveSundayParam, AllowLeaveSundaySource } from '@models/seahr/allow-leave-sunday';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class AllowLeaveSundayService {
  apiUrl = environment.apiUrl;

  dataSource = new BehaviorSubject<AllowLeaveSundaySource>(null)
  currentDataSource = this.dataSource.asObservable();
  constructor(private http: HttpClient) { }

  getPagination(pagination: Pagination, param: AllowLeaveSundayParam){
    let params = new HttpParams().appendAll({ ...pagination, ...param});
    return this.http.get<PaginationResult<AllowLeaveSundayDto>>(`${this.apiUrl}AllowLeaveSunday/GetPagination`, { params });
  }

  getEmployee(param: AllowLeaveSundayParam){
    let params = new HttpParams().appendAll({ ...param});
    return this.http.get<AllowLeaveSundayDto[]>(`${this.apiUrl}AllowLeaveSunday/GetEmployee`, { params });
  }

  getParts() {
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}AllowLeaveSunday/GetParts`);
  }

  allowLeave(empSelected: number[]){
    return this.http.put<OperationResult>(`${this.apiUrl}AllowLeaveSunday/AllowLeave`, empSelected);
  }

  disallow(empID: number){
    return this.http.put<OperationResult>(`${this.apiUrl}AllowLeaveSunday/DisallowLeave`, empID);
  }
}
