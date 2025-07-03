import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@environments/environment';
import { PageItemSettingParam } from '../../../../_helpers/params/page-item-setting.param';
import { Pagination, PaginationResult } from '../../../../_utilities/pagination-helper';
import { Observable } from 'rxjs';
import { eTM_Page_Item_Settings } from '../../../../_models/etm-page-item-settings';
import { KeyValuePair } from '../../../../_utilities/key-value-pair';
import { OperationResult } from '../../../../_utilities/operation-result';

@Injectable({ providedIn: 'root' })
export class PageItemSettingService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAll(param: PageItemSettingParam, pagination: Pagination): Observable<PaginationResult<eTM_Page_Item_Settings>> {
    var params = new HttpParams()
      .set('center_Level', param.center_Level ?? '')
      .set('tier_Level', param.tier_Level ?? '')
      .set('class_Level', param.class_Level ?? '')
      .set('pageNumber', pagination.pageNumber.toString())
      .set('pageSize', pagination.pageSize.toString());
    return this.http.get<PaginationResult<eTM_Page_Item_Settings>>(this.apiUrl + 'PageItemSetting/All', { params });
  }

  getCenterLevels(): Observable<KeyValuePair[]> {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'PageItemSetting/CenterLevels');
  }

  getTierLevels(center_Level: string): Observable<KeyValuePair[]> {
    var params = new HttpParams().appendAll({ center_Level });
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'PageItemSetting/TierLevels', { params });
  }

  getSections(center_Level: string, tier_Level: string): Observable<KeyValuePair[]> {
    var params = new HttpParams().appendAll({ center_Level, tier_Level });
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'PageItemSetting/Sections', { params });
  }

  getPages(): Observable<KeyValuePair[]> {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'PageItemSetting/Pages');
  }

  add(setting: eTM_Page_Item_Settings): Observable<OperationResult> {
    return this.http.post<OperationResult>(this.apiUrl + 'PageItemSetting', setting);
  }

  edit(setting: eTM_Page_Item_Settings): Observable<OperationResult> {
    return this.http.put<OperationResult>(this.apiUrl + 'PageItemSetting', setting);
  }
}