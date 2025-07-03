import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { DefectTop3 } from '../../../../_models/production/T2/CTB/defectop3';
import { FRIBADefect } from '../../../../_models/production/T2/CTB/fri-ba-defect';
import { T2CTBQuality } from '../../../../_models/production/T2/CTB/T2CTBQuality';


@Injectable({
  providedIn: 'root'
})
export class ProductionT2CTBQualityService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getData(tuCode: string, switchDate: boolean): Observable<T2CTBQuality> {
    let params = new HttpParams()
      .set('tuCode', tuCode)
      .set('switchDate', switchDate.toString())
    return this.http.get<T2CTBQuality>(this.baseUrl + 'ProductionT2CTBQuality/GetData', { params })
  }
  getDefectTop3Photos(tuCode: string, switchDate: boolean): Observable<Array<DefectTop3>> {
    let params = new HttpParams()
    .set('tuCode', tuCode)
    .set('switchDate', switchDate.toString())
    return this.http.get<Array<DefectTop3>>(this.baseUrl + 'ProductionT2CTBQuality/GetDefectTop3Photos', { params })
  }
  getDefectTop3Chart(tuCode: string, switchDate: boolean): Observable<Array<FRIBADefect>> {
    let params = new HttpParams()
    .set('tuCode', tuCode)
    .set('switchDate', switchDate.toString())
    return this.http.get<Array<FRIBADefect>>(this.baseUrl + 'ProductionT2CTBQuality/GetBADefectTop3Chart', { params })
  }

}
