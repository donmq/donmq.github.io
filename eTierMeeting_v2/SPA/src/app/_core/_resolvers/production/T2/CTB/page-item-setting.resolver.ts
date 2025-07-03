import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { PageItemSettingParam } from '../../../../_helpers/params/page-item-setting.param';
import { eTM_Page_Item_Settings } from '../../../../_models/etm-page-item-settings';
import { PageItemSettingService } from '../../../../_services/production/T2/CTB/page-item-setting.service';
import { Pagination, PaginationResult } from '../../../../_utilities/pagination-helper';

@Injectable({ providedIn: 'root' })
export class PageItemSettingResolver  {
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 10
  }
  param: PageItemSettingParam = <PageItemSettingParam>{};

  constructor(private pageItemSettingService: PageItemSettingService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<PaginationResult<eTM_Page_Item_Settings>> | Promise<PaginationResult<eTM_Page_Item_Settings>> | PaginationResult<eTM_Page_Item_Settings> {
    return this.pageItemSettingService.getAll(this.param, this.pagination);
  }
}