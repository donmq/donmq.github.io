import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { DataGrouped, EfficiencyKanban, EfficiencyKanbanParam, FactoryInfo } from '../../../../_models/production/T5/efficiency';

@Injectable({
  providedIn: 'root'
})
export class ProductionT5EfficiencyService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getData(param: EfficiencyKanbanParam): Observable<DataGrouped[]> {
    return this.http.post<DataGrouped[]>(this.baseUrl + 'EfficiencyKanban/getData', param)
  }
  getListFactory(): Observable<FactoryInfo[]> {
    return this.http.get<FactoryInfo[]>(this.baseUrl + 'EfficiencyKanban/getListFactory')
  }
}
