import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { ExportLeave } from '../../models/seahr/export-hp/export-leave';
import { ExportHPParam } from '@params/seahr/export-hp/export-hp-param';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { OperationResult } from '@utilities/operation-result';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ExportHpService {
  apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  dataSource = new BehaviorSubject<ExportHPParam>(null)
  currentDataSource = this.dataSource.asObservable();
  search(paramSearch: ExportHPParam, pagination: Pagination) {
    let params = new HttpParams().appendAll({ ...pagination });
    return this.http.post<PaginationResult<ExportLeave>>(`${this.apiUrl}ExportHP/GetData`, paramSearch, { params });
  }

  exportHPExcel(paramSearch: ExportHPParam, typeFile: string) {
    let params = new HttpParams().appendAll({ ...paramSearch, typeFile });
    return this.http.get<OperationResult>(`${this.apiUrl}ExportHP/ExportHPExcel`, { params });
  }
}
