import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { eTM_MES_PT1_Summary } from '../../../../_models/eTM_MES_PT1_Summary';
import { Efficiency } from '../../../../_models/production/efficiency';


@Injectable({
  providedIn: 'root'
})
export class EfficiencyService {
  baseUrl = environment.apiUrl;
  constructor(
    private http: HttpClient
  ) { }

  getData(deptId: string) {
    let params = new HttpParams().set('deptId', deptId);
    return this.http.get<Efficiency>(this.baseUrl + 'Efficiency', { params });
  }

  getDataChart(deptId: string) {
    let params = new HttpParams().set('deptId', deptId);
    return this.http.get<eTM_MES_PT1_Summary[]>(this.baseUrl + 'Efficiency/GetDataChart', { params });
  }
}
