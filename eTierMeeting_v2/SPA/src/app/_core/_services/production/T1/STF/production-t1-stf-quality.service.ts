import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { DefectTop3 } from '../../../../_models/production/T1/C2B/defectop3';
import { FRIBADefect } from '../../../../_models/production/T1/C2B/fri-ba-defect';
import { Quality } from '../../../../_models/production/T1/C2B/quality';


@Injectable({
  providedIn: 'root'
})
export class ProductionT1STFQualityService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getData(deptId: string) {
    let params = new HttpParams()
      .set('deptId', deptId.toString())
    return this.http.get<Quality>(this.baseUrl + 'ProductionT1STFQuality/getData', { params })
  }
  GetDefectTop3(deptId: string) {
    let params = new HttpParams()
      .set('deptId', deptId.toString())
    return this.http.get<Array<DefectTop3>>(this.baseUrl + 'ProductionT1STFQuality/getDefectTop3', { params })
  }
}
