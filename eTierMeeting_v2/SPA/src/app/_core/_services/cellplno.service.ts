import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../environments/environment';
import { CellPlno } from '../_models/cell-plno';
import { Hpa15 } from '../_models/hp-a15';
import { OperationResult } from '../_models/operation-result';
import { Pagination, PaginationResult } from '../_models/pagination';

const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class CellPlnoService {
  url = environment.apiUrl;
  constructor(private http: HttpClient,
    private _spinnerService: NgxSpinnerService) { }

  getAllCellPlno() {
    return this.http.get<Hpa15[]>(`${API}CellPlno/GetAllCellPlno`);
  }

  getListPlnoByCellID(cellID: string) {
    return this.http.get<Hpa15[]>(`${API}CellPlno/GetListPlnoByCellIDV2?cellCode=${cellID}`);
  }

  getListPlnoByPDCID(pdcID: number) {
    return this.http.get<Hpa15[]>(`${API}CellPlno/GetListPlnoByPDCID?pdcID=${pdcID}`);
  }

  getListPlnoByBuildingID(buildingID: number) {
    return this.http.get<Hpa15[]>(`${API}CellPlno/GetListPlnoByBuildingID?buildingID=${buildingID}`);
  }

  getListPlnoByMultipleBuildingID(listBuildingID: string[]) {
    return this.http.post<Hpa15[]>(`${API}CellPlno/GetListPlnoByMultipleBuildingID`, listBuildingID);
  }

  getListPlnoByMultipleCellID(listCellID: string[]) {
    return this.http.post<Hpa15[]>(`${API}CellPlno/GetListPlnoByMultipleCellID`, listCellID);
  }
  getListPlnoByMultipleID(listAll: any) {
    return this.http.post<Hpa15[]>(`${API}CellPlno/GetListPlnoByMultipleID`, listAll);
  }

  getListPlnoByBuildingAndCellID(buildingID: number, cellCode: string) {
    return this.http.get<Hpa15[]>(`${API}CellPlno/GetListPlnoByBuildingAndCellID?buildingID=${buildingID}&cellCode=${cellCode}`);
  }

  getListCellPlno(pagination: Pagination) {
    let params = new HttpParams();
    if (pagination.currentPage !== null && pagination.pageSize !== null) {
      params = params.append('pageNumber', pagination.currentPage.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }
    return this.http.get<PaginationResult<CellPlno>>(`${API}CellPlno/GetListCellPlno`, { params });
  }

  searchCellPlno(keyword: string, pagination: Pagination) {
    let params = new HttpParams();
    if (pagination.currentPage !== null && pagination.pageSize !== null) {
      params = params.append('pageNumber', pagination.currentPage.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }
    return this.http.post<PaginationResult<CellPlno>>(`${API}CellPlno/SearchCellPlno?keyword=${keyword}`, {}, { params });
  }

  removeCellPlno(cellPlno: CellPlno, lang: string) {
    return this.http.post<OperationResult>(`${API}CellPlno/RemoveCellPlno?lang=${lang}`, cellPlno, {});
  }

  addCellPlno(cellPlno: CellPlno, lang: string) {
    return this.http.post<OperationResult>(`${API}CellPlno/AddCellPlno?lang=${lang}`, cellPlno, {});
  }

  updateCellPlno(cellPlno: CellPlno, lang: string) {
    return this.http.post<OperationResult>(`${API}CellPlno/UpdateCellPlno?lang=${lang}`, cellPlno, {});
  }

  exportExcelData() {
    this._spinnerService.show();
    return this.http.post(`${API}CellPlno/ExportExcel`, {}, { responseType: 'blob', withCredentials: true }).subscribe((result: Blob) => {
      if (result.type !== 'application/xlsx') {
        alert(result.type);
      }
      const blob = new Blob([result]);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      const currentTime = new Date();
      const filename =
        'CellPlnoExport-' +
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
