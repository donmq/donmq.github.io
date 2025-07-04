import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { DataGrouped, EfficiencyKanban, EfficiencyKanbanParam, FactoryInfo } from '../../../../_models/production/T6/efficiency';

@Injectable({
  providedIn: 'root'
})
export class ProductionT6EfficiencyService {
  baseUrl = environment.apiUrl + 'EfficiencyKanbanT6/';
  constructor(private http: HttpClient) { }

  getData(param: EfficiencyKanbanParam): Observable<DataGrouped[]> {
    return this.http.post<DataGrouped[]>(this.baseUrl + 'getData', param)
  }
  getListFactory(): Observable<FactoryInfo[]> {
    return this.http.get<FactoryInfo[]>(this.baseUrl + 'getListFactory')
  }
  getListBrand(): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + 'getListBrand')
  }
}
