import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { DefectTop3 } from '../../../../_models/production/T1/UPF/defectTop3';
import { Quality } from '../../../../_models/production/T1/UPF/quality';

@Injectable({
  providedIn: 'root'
})
export class ProductionT1UpfQualityService {
  apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getData = (deptId: string) => {
    let params = new HttpParams().set("deptId",deptId);
    return this.http.get<Quality>(this.apiUrl + "ProductionT1UPFQuality/GetData", { params});
  }

  getDefectTop3 = (deptId: string) => {
    let params = new HttpParams().set("deptId",deptId);
    return this.http.get<DefectTop3[]>(this.apiUrl + "ProductionT1UPFQuality/GetDefectTop3",{ params});
  }

}
