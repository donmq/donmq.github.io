import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { eTM_Video } from '../../../../_models/eTM_Video';
import { ProductionT1SafetyService } from '../../../../_services/production/T1/C2B/production-t1-safety.service';

@Injectable({
  providedIn: 'root'
})
export class ProductionT1C2BSafetyResolver  {
  constructor(private productionT1SafetyService: ProductionT1SafetyService) {
  }

  resolve(route: ActivatedRouteSnapshot): Observable<eTM_Video[]> {
    return this.productionT1SafetyService.getTodayData(route.params.deptId);
  }
}
