import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { eTM_Video } from '../../../../_models/eTM_Video';
import { ProductionT1UPFKaizenService } from '../../../../_services/production/T1/UPF/production-t1-upf-kaizen.service';

@Injectable({
  providedIn: 'root'
})
export class ProductionT1UPFKaizenResolver  {
  constructor(private productionT1UPFKaizenService : ProductionT1UPFKaizenService){}
  
  resolve(route: ActivatedRouteSnapshot): Observable<eTM_Video[]> {
    return this.productionT1UPFKaizenService.getListVideo(route.params.deptId);
  }
}
