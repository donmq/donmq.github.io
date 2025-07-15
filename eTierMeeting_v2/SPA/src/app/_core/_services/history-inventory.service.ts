import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../environments/environment';
import { HistoryInventoryParams } from '../_dtos/history-inventory-params';
import { DataHistoryInventory } from '../_models/data-history-inventory';
import { HistoryInventory } from '../_models/history-inventory';
import { Pagination, PaginationResult } from '../_models/pagination';

const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class HistoryInventoryService {


  constructor(private http: HttpClient,
    private _spinnerService: NgxSpinnerService) { }

  searchHistoryInventory(pagination: Pagination, historyInventoryParams: HistoryInventoryParams) {
    let params = new HttpParams();
    if (pagination.currentPage !== null && pagination.pageSize !== null) {
      params = params.append('pageNumber', pagination.currentPage.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }
    return this.http.post<PaginationResult<HistoryInventory>>(`${API}HistoryInventory/SearchHistoryInventory`, historyInventoryParams,
      { params: params });
  }

  getDetailHistoryInventory(historyInventoryID: number) {
    return this.http.post<DataHistoryInventory>
      (`${API}HistoryInventory/GetDetailHistoryInventory?historyInventoryID=${historyInventoryID}`, {});
  }

  exportPDF(historyInventory: HistoryInventory) {
    this._spinnerService.show();
    return this.http.post(`${API}HistoryInventory/ExportFile`, historyInventory, { responseType: 'blob', withCredentials: true }).subscribe((result: Blob) => {
      if (result.type !== 'application/pdf') {
        alert(result.type);
      }
      const blob = new Blob([result]);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      const currentTime = new Date();
      const filename =
        'ExportHistoryInventory-' +
        currentTime.getFullYear().toString() +
        (currentTime.getMonth() + 1) + '_' +
        currentTime.getDate() +
        currentTime
          .toLocaleTimeString()
          .replace(/[ ]|[,]|[:]/g, '')
          .trim() +
        '.pdf';
      link.href = url;
      link.setAttribute('download', filename);
      document.body.appendChild(link);
      link.click();
      this._spinnerService.hide();
    });
  }

  exportExcel(historyInventory: HistoryInventory) {
    this._spinnerService.show();
    return this.http.post(`${API}HistoryInventory/ExportFile`, historyInventory, { responseType: 'blob', withCredentials: true }).subscribe((result: Blob) => {
      if (result.type !== 'application/xlsx') {
        alert(result.type);
      }
      const blob = new Blob([result]);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      const currentTime = new Date();
      const filename =
        'ExportHistoryInventory-' +
        currentTime.getFullYear().toString() +
        (currentTime.getMonth() + 1) + '_' +
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

  exportPdfByDay(date: string) {
    this._spinnerService.show();
    return this.http.get(`${API}HistoryInventory/GetDataPdfByDay?date=${date}`, { responseType: 'blob', withCredentials: true }).subscribe((result: Blob) => {
      if (result.type !== 'application/xlsx') {
        alert(result.type);
      }
      const blob = new Blob([result]);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      const currentTime = new Date();
      const filename =
        'ExportHistoryInventoryPDF-' +
        currentTime.getFullYear().toString() +
        (currentTime.getMonth() + 1) + '_' +
        currentTime.getDate() +
        currentTime
          .toLocaleTimeString()
          .replace(/[ ]|[,]|[:]/g, '')
          .trim() +
        '.pdf';
      link.href = url;
      link.setAttribute('download', filename);
      document.body.appendChild(link);
      link.click();
      this._spinnerService.hide();
    });
  }

  dateCheck(checkDate: string) {
    return this.http.post(`${API}HistoryInventory/CheckDate?checkDate=${checkDate}`, {});
  }

}
