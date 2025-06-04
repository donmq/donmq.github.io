import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { LunchBreak } from '@models/common/lunch-break';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LunchBreakService {
  apiUrl = environment.apiUrl + 'LunchBreak';
  lunchBreakEmitter: EventEmitter<boolean> = new EventEmitter<boolean>();
  lunchBreakSource = new BehaviorSubject<LunchBreak>(null);
  currentlunchBreak = this.lunchBreakSource.asObservable();

  constructor(
    private http: HttpClient,
  ) { }

  create(dto: LunchBreak) {
    return this.http.post<OperationResult>(`${this.apiUrl}/Create`, dto);
  }

  update(dto: LunchBreak) {
    return this.http.put<OperationResult>(`${this.apiUrl}/Update`, dto);
  }

  delete(id: number) {
    return this.http.delete<OperationResult>(`${this.apiUrl}/Delete`, { params: { id } });
  }

  getDataPagination(pagination: Pagination) {
    return this.http.get<PaginationResult<LunchBreak>>(`${this.apiUrl}/GetDataPagination`, { params: { 'PageNumber': pagination.pageNumber, 'PageSize': pagination.pageSize } });
  }

  getDetail(id: number) {
    return this.http.get<LunchBreak>(`${this.apiUrl}/GetDetail`, { params: { id } });
  }

  getListLunchBreak() {
    return this.http.get<LunchBreak[]>(`${this.apiUrl}/GetListLunchBreak`, {});
  }

  emitDataChange(check: boolean) {
    this.lunchBreakEmitter.emit(check);
  }

  changeData(item: LunchBreak) {
    this.lunchBreakSource.next(item);
  }
}
