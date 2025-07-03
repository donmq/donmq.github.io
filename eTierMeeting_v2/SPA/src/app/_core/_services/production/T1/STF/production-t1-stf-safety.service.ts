import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { eTM_Video } from '../../../../_models/production/T1/STF/eTM_Video';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ProductionT1STFSafetyService {
  private apiUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getTodayData(deptId: string): Observable<eTM_Video[]> {
    return this.http.get<eTM_Video[]>(`${this.apiUrl}ProductionT1STFSafety/TodayData`, { params: { deptId } });
  }
}