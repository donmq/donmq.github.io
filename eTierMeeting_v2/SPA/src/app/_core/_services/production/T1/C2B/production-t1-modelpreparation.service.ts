import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { eTM_Video } from '../../../../_models/eTM_Video';

@Injectable({
  providedIn: 'root'
})
export class ProductionT1ModelpreparationService {
  private apiUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getModelPreparation(deptId: string) {
    return this.http.get<eTM_Video[]>(`${this.apiUrl}ProductionT1ModelPreparation/GetModelPreparation`, { params: { deptId } });
  }
}
