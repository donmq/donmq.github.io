import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { eTM_Video } from '../../../../_models/eTM_Video';


@Injectable({ providedIn: 'root' })
export class ProductionT1SafetyService {
  private apiUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getTodayData(deptId: string) {
    return this.http.get<eTM_Video[]>(`${this.apiUrl}ProductionT1Safety/TodayData`, { params: { deptId } });
  }
}