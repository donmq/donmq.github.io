import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { eTM_Video } from '../../../../_models/production/T1/STF/eTM_Video';
import { ProductionT1Video } from '../../../../_models/productionT1Video';


@Injectable({
  providedIn: 'root'
})
export class ProductionT1STFKaizenService {
  baseUrl = environment.apiUrl;
  constructor(
    private http: HttpClient
  ) { }

  getListVideo(deptId: string, date: string): Observable<eTM_Video[]> {
    let params = new HttpParams().set('deptId', deptId).set('date', date);
    return this.http.get<eTM_Video[]>(this.baseUrl + 'ProductionT1STFKaizen', { params });
  }
}
