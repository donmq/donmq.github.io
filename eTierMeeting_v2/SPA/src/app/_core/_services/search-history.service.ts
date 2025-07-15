import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../environments/environment';
import { Pagination, PaginationResult } from '../_models/pagination';
import { SearchHistory } from '../_models/search-history';


const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class SearchHistoryService {

  constructor(private http: HttpClient,
    private _spinnerService: NgxSpinnerService) { }

  searchHistory(pagination: Pagination, searchHistoryParams: any) {
    let params = new HttpParams();
    if (pagination.currentPage !== null && pagination.pageSize !== null) {
      params = params.append('pageNumber', pagination.currentPage.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }
    return this.http.post<PaginationResult<SearchHistory>>(`${API}History/SearchHistory`, searchHistoryParams, { params: params });
  }


  exportExcelData(searchHistoryParams: any) {
    this._spinnerService.show();
    return this.http.post(`${API}History/ExportExcel`, searchHistoryParams, {
      responseType: 'blob'
    }).subscribe((result: Blob) => {
      if (result.type !== 'application/xlsx') {
        alert(result.type);
      }
      const blob = new Blob([result]);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      const currentTime = new Date();
      const filename =
        'DataMachine' +
        currentTime.getFullYear().toString() +
        (currentTime.getMonth() + 1) +
        currentTime.getDate() +
        currentTime
          .toLocaleTimeString()
          .replace(/[ ]|[,]|[:]/g, '')
          .trim() +
        '.xlsx';
      link.href = url;
      link.setAttribute('download', filename);
      document.body.appendChild(link);
      link.click();
      this._spinnerService.hide();
    });
  }

}
