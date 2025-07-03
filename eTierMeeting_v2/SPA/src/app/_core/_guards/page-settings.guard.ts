import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { PageEnableDisableService } from '../_services/page-enable-disable.service';

@Injectable({ providedIn: 'root' })
export class PageSettingsGuard  {
  constructor(private pageEnableDisableService: PageEnableDisableService) { }

  async canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    let class_Level: string = route.data['class_Level'];
    let dept_Id: string = route.params['deptId']
    let nav: string = route.queryParams['nav'] ?? 'next';
    let url: string = state.url;
    return await this.pageEnableDisableService.checkGuard(class_Level, dept_Id, nav, url);
  }
}