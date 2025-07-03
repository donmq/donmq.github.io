import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { ChartData, EfficiencyChart } from '../../../../_models/production/T2/CTB/efficiency';

@Injectable({
  providedIn: 'root'
})
export class ProductionT2CTBEfficiencyService {
  baseUrl = environment.apiUrl;
  constructor(
    private http: HttpClient
  ) { }

  getData(deptId: string, param: string): Observable<EfficiencyChart> {
    let params = new HttpParams()
      .set('check', param)
      .set('deptId', deptId);
    return this.http.get<EfficiencyChart>(this.baseUrl + 'ProductionT2CTBEfficiency', { params });
  }

}
