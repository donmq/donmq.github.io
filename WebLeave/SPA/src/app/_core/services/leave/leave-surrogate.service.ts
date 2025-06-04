import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { Surrogate, SurrogateParam, SurrogateRemove } from '@models/leave/surrogate';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LeaveSurrogateService {
  apiUrl = `${environment.apiUrl}LeaveSurrogate`;
  constructor(private http: HttpClient) { }

  getDataPagination(pagination: PaginationParam, param: SurrogateParam): Observable<PaginationResult<Surrogate>> {
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<Surrogate>>(`${this.apiUrl}/GetDataPagination`, { params });
  }

  getDetail(userId: number): Observable<Surrogate> {
    return this.http.get<Surrogate>(`${this.apiUrl}/GetDetail`, { params: { userId } });
  }

  getSurrogates(userId: number): Observable<KeyValuePair[]> {
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}/GetSurrogates`, { params: { userId } });
  }

  saveSurrogate(surrogate: Surrogate): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.apiUrl}/SaveSurrogate`, surrogate);
  }

  removeSurrogate(removes: SurrogateRemove): Observable<OperationResult> {
    return this.http.put<OperationResult>(`${this.apiUrl}/RemoveSurrogate`, removes);
  }
}
