import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { eTM_HSE_Score_Image } from '../../../../_models/production/T2/CTB/eTM_HSE_Score_Image';
import { SefetyViewModel } from '../../../../_models/production/T2/CTB/sefetyViewModel';

@Injectable({
  providedIn: 'root'
})
export class ProductionT2SafetyService {
  apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getData(building: string) {
    return this.http.get<SefetyViewModel>(`${this.apiUrl}ProductionT2Safety/GetData`, { params: { building } });
  }

  getDetailScoreUnPass(hseScoreID: number) {
    let params = new HttpParams().set('hseScoreID', hseScoreID.toString());
    return this.http.get<eTM_HSE_Score_Image[]>(`${this.apiUrl}ProductionT2Safety/GetDetailScoreUnPass`, { params });
  }
}
