import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { ReportChartDto } from '@models/report/report-chart-dto';

@Injectable({
  providedIn: 'root'
})
export class ReportChartService {
  apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  getData() {
    return this.http.get<ReportChartDto>(this.apiUrl + 'ReportChart/getData');
  }
  getDataChartInArea(areaId: number) {
    let params = new HttpParams().set('areaId', areaId);
    return this.http.get<ReportChartDto>(this.apiUrl + 'ReportChart/GetDataChartInArea', {params});
  }
}
