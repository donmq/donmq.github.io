import { leaveDataHistory } from '@models/leave/leave-history/leaveDataHistory';
import { HistoryExportParam, SearchHistoryParams } from '@models/leave/leave-history/searchHistoryParams';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { Pagination } from '@utilities/pagination-utility';
import { BehaviorSubject, Observable } from 'rxjs';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class LeaveHistoryService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }
  dataSource = new BehaviorSubject<SearchHistoryParams>(null)
  currentDataSource = this.dataSource.asObservable();

  getCategory(lang: string) {
    let params = new HttpParams()
      .set('lang', lang);
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}LeaveHistory/GetListCategory`, {params});
  }

  search(param: SearchHistoryParams, pagination: Pagination): Observable<leaveDataHistory> {
    const request = { ...param };
    request.categoryId = request.categoryId ?? -1;
    request.userID = request.userID ?? -1;
    request.empId = request.empId ?? '';
    request.startTime = request.startTime ?? '';
    request.endTime = request.endTime ?? '';
    request.status = request.status ?? -1;
    let params = new HttpParams().appendAll({ ...request, ...pagination });
    return this.http.get<leaveDataHistory>(this.baseUrl + 'LeaveHistory/Search', { params });
  }

  exportExcelHistory(param: HistoryExportParam, pagination: Pagination) {
    let params = new HttpParams().appendAll({ ...param, ...pagination });

    return this.http.get<OperationResult>(this.baseUrl + 'LeaveHistory/ExportExcel', { params })
  }

}
