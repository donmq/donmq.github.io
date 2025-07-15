import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../environments/environment';
import { Cell } from '../_models/cell';
import { OperationResult } from '../_models/operation-result';
import { Pagination, PaginationResult } from '../_models/pagination';

const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class CellService {

  constructor(private http: HttpClient,
    private _spinnerService: NgxSpinnerService) { }


  getAllCellAdmin() {
    return this.http.get<Cell[]>(`${API}Cell/GetAllCellAdmin`);
  }

  getAllCell() {
    return this.http.get<Cell[]>(`${API}Cell/GetAllCell`);
  }

  getDataCell(cellCode) {
    return this.http.get<Cell>(`${API}Cell/GetDataCell?cellCode=${cellCode}`);
  }
  getListCellByBuildingID(buildingID: number) {
    return this.http.get<Cell[]>(`${API}Cell/GetListCellByBuildingID?buildingID=${buildingID}`);
  }
  getListCellExistPlnoByBuildingID(buildingID: number) {
    return this.http.get<Cell[]>(`${API}Cell/GetListCellExistPlnoByBuildingID?buildingID=${buildingID}`);
  }

  getListCellByPdcID(pdcID: number) {
    return this.http.get<Cell[]>(`${API}Cell/GetListCellByPdcID?pdcId=${pdcID}`);
  }

  getListCell(pagination: Pagination) {
    let params = new HttpParams();
    if (pagination.currentPage !== null && pagination.pageSize !== null) {
      params = params.append('pageNumber', pagination.currentPage.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }
    return this.http.get<PaginationResult<Cell>>(`${API}Cell/GetListCell`, { params });
  }

  searchCell(keyword: string, pagination: Pagination) {
    let params = new HttpParams();
    if (pagination.currentPage !== null && pagination.pageSize !== null) {
      params = params.append('pageNumber', pagination.currentPage.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }
    return this.http.post<PaginationResult<Cell>>(`${API}Cell/SearchCell?keyword=${keyword}`, {}, { params });
  }

  removeCell(cell: Cell, lang: string) {
    return this.http.post<OperationResult>(`${API}Cell/RemoveCell?lang=${lang}`, cell, {});
  }

  addCell(cell: Cell, lang: string) {
    return this.http.post<OperationResult>(`${API}Cell/AddCell?lang=${lang}`, cell, {});
  }

  updateCell(cell: Cell, lang: string) {
    return this.http.post<OperationResult>(`${API}Cell/UpdateCell?lang=${lang}`, cell, {});
  }

  exportExcelData() {
    this._spinnerService.show();
    return this.http.post(`${API}Cell/ExportExcel`, {}, {
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
        'CellExport-' +
        currentTime.getFullYear().toString() +
        (currentTime.getMonth() + 1) +
        currentTime.getDate() +
        '.xlsx';
      link.href = url;
      link.setAttribute('download', filename);
      document.body.appendChild(link);
      link.click();
      this._spinnerService.hide();
    });
  }

}
