import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { OperationResult } from '../_models/operation-result';
import { Pagination, PaginationResult } from '../_models/pagination';
import { Pdc } from '../_models/pdc';

const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class PdcService {

  constructor(private http: HttpClient) { }

  getAllPdc() {
    return this.http.get<Pdc[]>(`${API}PDC/GetAllPDC`);
  }

  searchPDC(keyword: string, pagination: Pagination) {
    let params = new HttpParams();
    if (pagination.currentPage !== null && pagination.pageSize !== null) {
      params = params.append('pageNumber', pagination.currentPage.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }
    return this.http.post<PaginationResult<Pdc>>(`${API}PDC/SearchPDC?keyword=${keyword}`, {}, { params: params });
  }

  getListAllPDC(pagination: Pagination) {
    let params = new HttpParams();
    if (pagination.currentPage !== null && pagination.pageSize !== null) {
      params = params.append('pageNumber', pagination.currentPage.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }
    return this.http.get<PaginationResult<Pdc>>(`${API}PDC/GetListAllPDC`, { params: params });
  }

  removePDC(pdc: Pdc, lang: string) {
    return this.http.post<OperationResult>(`${API}PDC/RemovePDC?lang=${lang}`, pdc, {});
  }

  addPDC(pdc: Pdc, lang: string) {
    return this.http.post<OperationResult>(`${API}PDC/AddPDC?lang=${lang}`, pdc, {});
  }

  updatePDC(pdc: Pdc, lang: string) {
    return this.http.post<OperationResult>(`${API}PDC/UpdatePDC?lang=${lang}`, pdc, {});
  }
}
