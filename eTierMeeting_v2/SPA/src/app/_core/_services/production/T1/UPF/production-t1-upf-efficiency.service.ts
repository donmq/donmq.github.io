import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Efficiency } from '../../../../_models/production/efficiency';


@Injectable({
  providedIn: 'root'
})
export class ProductionT1UPFEfficiencyService {
  baseUrl = environment.apiUrl;
  constructor(
    private http: HttpClient
  ) { }

  getData(deptId: string): Observable<Efficiency[]> {
    return this.http.get<Efficiency[]>(this.baseUrl + 'ProductionT1UPFEfficiency', { params: {deptId} });
  }
}
