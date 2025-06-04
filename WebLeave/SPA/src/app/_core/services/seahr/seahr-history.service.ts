import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '@env/environment';
import { SearchHistoryParams } from '@params/seahr/seahr-history/search-history-params';
import { KeyValuePair } from '@utilities/key-value-pair';
import { Pagination } from '@utilities/pagination-utility';
import { OperationResult } from '@utilities/operation-result';
import { SeaHistorySearch } from '@models/seahr/sea-hr-history';

@Injectable({
  providedIn: 'root'
})
export class SeahrHistoryService {
  apiUrl = environment.apiUrl;
  dataSource = new BehaviorSubject<SearchHistoryParams>(null)
  currentDataSource = this.dataSource.asObservable();
  constructor(private http: HttpClient) { }
  //call API
  getCategory(lang: string) {
    let params = new HttpParams()
      .set('lang', lang);
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}SeaHrHistory/GetCategory`, { params });
  }
  getDepartments() {
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}SeaHrHistory/GetDepartments`,);
  }
  getPart(ID: number) {
    let params = new HttpParams()
      .set('ID', ID.toString());
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}SeaHrHistory/GetPart`, { params });
  }
  getLeaveData(paramsSearch: SearchHistoryParams, pagination: Pagination)
    : Observable<SeaHistorySearch> {
    let paramsTmp = { ...paramsSearch, ...pagination };
    let params = new HttpParams().appendAll(paramsTmp);
    return this.http.get<SeaHistorySearch>(this.apiUrl + 'SeaHrHistory/GetLeaveData', { params });
  }

  exportExcel(paramsSearch: SearchHistoryParams, pagination: Pagination) {
    let params = new HttpParams().appendAll({ ...paramsSearch, ...pagination});
    return this.http.get<OperationResult>(`${this.apiUrl}SeaHrHistory/ExportExcelAll`, { params }).pipe(tap())
  }

}
