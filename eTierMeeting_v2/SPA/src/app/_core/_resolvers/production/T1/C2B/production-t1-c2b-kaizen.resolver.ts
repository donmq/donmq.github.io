import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { eTM_Video } from '../../../../_models/eTM_Video';
import { ProductionT1KaizenService } from '../../../../_services/production/T1/C2B/production-t1-kaizen.service';

@Injectable({
  providedIn: 'root'
})
export class ProductionT1C2BKaizenResolver  {
  constructor(private productionT1KaizenService: ProductionT1KaizenService){}

  resolve(route: ActivatedRouteSnapshot): Observable<eTM_Video[]> {
    return this.productionT1KaizenService.getListVideo(route.params.deptId);
  }
}
