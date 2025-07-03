import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { eTM_Video } from '../../../../_models/eTM_Video';


@Injectable({
  providedIn: 'root'
})
export class ProductionT1KaizenService {
  baseUrl = environment.apiUrl;
  constructor(
    private http: HttpClient
  ) { }

  getListVideo(deptId: string) {
    let params = new HttpParams().set('deptId', deptId);
    return this.http.get<eTM_Video[]>(this.baseUrl + 'ProductionT1Kaizen', { params });
  }
}
