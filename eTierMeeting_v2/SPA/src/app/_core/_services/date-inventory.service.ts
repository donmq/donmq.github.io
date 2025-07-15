import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lang } from 'moment';
import { environment } from '../../../environments/environment';
import { AddDateInventory } from '../_models/add-date-inventory';
import { DateInventory } from '../_models/date-inventory';
import { OperationResult } from '../_models/operation-result';
import { Pagination, PaginationResult } from '../_models/pagination';

const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class DateInventoryService {

  constructor(private http: HttpClient) { }

  removeDateInventory(id: number, lang: string) {
    return this.http.post<OperationResult>(`${API}DateInventory/RemoveDateInventory?id=${id}&lang=${lang}`, {});
  }

  addDateInventory(addDateInventory: AddDateInventory, lang: string) {
    return this.http.post<OperationResult>(`${API}DateInventory/AddDateInventory?lang=${lang}`, addDateInventory);
  }

  GetAllDateInventories(pagination: Pagination) {
    let params = new HttpParams();
    if (pagination.currentPage !== null && pagination.pageSize !== null) {
      params = params.append('pageNumber', pagination.currentPage.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }
    return this.http.post<PaginationResult<DateInventory>>(`${API}DateInventory/GetAllDateInventories`, {}, { params });
  }

  checkScheduleInventory() {
    return this.http.get<any>(`${API}DateInventory/Check`);
  }

}
