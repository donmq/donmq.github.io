import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Building } from '../_models/building';
import { OperationResult } from '../_models/operation-result';
import { Pagination, PaginationResult } from '../_models/pagination';

const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class BuildingService {

  constructor(private http: HttpClient) { }

  getAllBuilding() {
    return this.http.get<Building[]>(`${API}Building/GetAllBuilding`);
  }

  getBuildingByPdcID(pdcID: number) {
    return this.http.get<Building[]>(`${API}Building/GetBuildingByPdcID?idPDC=${pdcID}`);
  }

  getBuildingByCellID(cellID: number) {
    return this.http.get<Building[]>(`${API}Building/GetBuildingByCellID?cellID=${cellID}`);
  }

  getBuildingByCellCodeAndPDC(cellCode: string, pdcID: number = 0) {
    return this.http.get<Building[]>(`${API}Building/GetBuildingByCellCodeAndPDC?cellCode=${cellCode}&idPDC=${pdcID}`);
  }

  getListBuilding(pagination: Pagination) {
    let params = new HttpParams();
    if (pagination.currentPage !== null && pagination.pageSize !== null) {
      params = params.append('pageNumber', pagination.currentPage.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }
    return this.http.get<PaginationResult<Building>>(`${API}Building/GetListBuilding`, { params });
  }

  searchBuilding(keyword: string, pagination: Pagination) {
    let params = new HttpParams();
    if (pagination.currentPage !== null && pagination.pageSize !== null) {
      params = params.append('pageNumber', pagination.currentPage.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }
    return this.http.post<PaginationResult<Building>>(`${API}Building/SearchBuilding?keyword=${keyword}`, {}, { params });
  }

  removeBuilding(building: Building, lang: string) {
    return this.http.post<OperationResult>(`${API}Building/RemoveBuilding?lang=${lang}`, building, {});
  }

  addBuilding(building: Building, lang: string) {
    return this.http.post<OperationResult>(`${API}Building/AddBuilding?lang=${lang}`, building, {});
  }

  updateBuilding(building: Building, lang: string) {
    return this.http.post<OperationResult>(`${API}Building/UpdateBuilding?lang=${lang}`, building, {});
  }
}
