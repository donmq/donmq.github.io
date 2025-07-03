import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { DefectTop3 } from '../../../../_models/production/T1/C2B/defectop3';
import { FRIBADefect } from '../../../../_models/production/T1/C2B/fri-ba-defect';
import { Quality } from '../../../../_models/production/T1/C2B/quality';


@Injectable({
  providedIn: 'root'
})
export class QualityService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getData(deptId: string) {
    let params = new HttpParams()
      .set('deptId', deptId.toString())
    return this.http.get<Quality>(this.baseUrl + 'Quality/getData', { params })
  }
  GetDefectTop3(deptId: string) {
    let params = new HttpParams()
      .set('deptId', deptId.toString())
    return this.http.get<Array<DefectTop3>>(this.baseUrl + 'Quality/getDefectTop3', { params })
  }
  GetDefectTop3Chart(deptId: string) {
    let params = new HttpParams()
      .set('deptId', deptId.toString())
    return this.http.get<Array<FRIBADefect>>(this.baseUrl + 'Quality/getBADefectTop3Chart', { params })
  }

}
