import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { InventoryLine } from '../_models/inventoryLine';
import { InventoryService } from '../_services/inventory.service';

@Injectable()
export class InventoryGetLineResolve implements Resolve<InventoryLine[]> {

  constructor(private _inventoryService: InventoryService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<InventoryLine[]> | Promise<InventoryLine[]> | InventoryLine[] {
    const buildId = route.paramMap.get('building');
    const chekgetdata = route.paramMap.get('check');
    return this._inventoryService.getCellInevntory(buildId, chekgetdata);
  }
}
