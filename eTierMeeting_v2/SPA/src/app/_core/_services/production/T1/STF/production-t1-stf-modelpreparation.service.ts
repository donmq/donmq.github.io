import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { ProductionT1Video } from '../../../../_models/production-t1-video';
import { eTM_Video } from '../../../../_models/production/T1/STF/eTM_Video';

@Injectable({
  providedIn: 'root'
})
export class ProductionT1STFModelpreparationService {
  private apiUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getModelPreparation(deptId: string): Observable<eTM_Video[]> {
    return this.http.get<eTM_Video[]>(`${this.apiUrl}ProductionT1STFModelPreparation/GetModelPreparation`, { params: { deptId } }).pipe(
      tap(res => {
        res.forEach(item => {
          // item.play_Date = item.play_Date?.toDate();
          // item.update_At = item.update_At?.toDate();
          // item.insert_At = item.insert_At?.toDate();
        });
      })
    );
  }
}
