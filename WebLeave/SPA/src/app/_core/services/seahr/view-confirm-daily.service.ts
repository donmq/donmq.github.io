import { HttpParams } from '@angular/common/http';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { ViewConfirmDaily } from '@models/seahr/view-confirm-daily.model';
import * as moment from 'moment';
import { BehaviorSubject, tap } from 'rxjs';
import { OperationResult } from '@utilities/operation-result';
import { ViewConfirmDailyParam } from '@params/seahr/view-confirm-daily-param';

@Injectable({
  providedIn: 'root'
})
export class ViewConfirmDailyService {
  apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  dataSource = new BehaviorSubject<ViewConfirmDailyParam>(null)
  currentDataSource = this.dataSource.asObservable();
  getViewConfirmDaily(param: ViewConfirmDailyParam, pagination: Pagination) {
    let params = new HttpParams().appendAll({ ...param, ...pagination });
    return this.http.get<PaginationResult<ViewConfirmDaily>>(`${this.apiUrl}ViewConfirmDaily/GetViewConfirmDaily`, { params });
  }

  exportExcel(param: ViewConfirmDailyParam) {
    let params = new HttpParams().appendAll({ ...param });

    return this.http.get<OperationResult>(`${this.apiUrl}ViewConfirmDaily/ExportExcel`, { params });
  }
}
