import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { eTM_Video } from '../../../../_models/eTM_Video';
import { ProductionT1ModelpreparationService } from '../../../../_services/production/T1/C2B/production-t1-modelpreparation.service';

@Injectable({
  providedIn: 'root'
})
export class ProductionT1C2BModelPreparationResolver  {
  constructor(private productionT1ModelpreparationService: ProductionT1ModelpreparationService) { }
  resolve(route: ActivatedRouteSnapshot): Observable<eTM_Video[]> { 
    return this.productionT1ModelpreparationService.getModelPreparation(route.params.deptId);
  }
}
