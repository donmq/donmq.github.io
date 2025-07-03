import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { eTM_Video } from '../../../../_models/eTM_Video';
import { ProductionT1UPFModelpreparationService } from '../../../../_services/production/T1/UPF/production-t1-upf-modelpreparation.service';

@Injectable({
  providedIn: 'root'
})
export class ProductionT1UPFModelPreparationResolver  {
  constructor(private service: ProductionT1UPFModelpreparationService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<eTM_Video[]> {
    return this.service.getListVideo(route.params.deptId);
  }
}
