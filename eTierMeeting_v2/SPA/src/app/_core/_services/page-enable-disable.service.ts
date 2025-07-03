import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { KeyValuePair } from '../_utilities/key-value-pair';
import { Observable } from 'rxjs';
import { eTM_Page_Settings } from '../_models/etm-page-settings';
import { PageEnableDisableParam } from '../_helpers/params/page-enable-disable.param';
import { OperationResult } from '../_utilities/operation-result';
import { Router } from '@angular/router';
import { LocalStorageConstants } from '@constants/storage.constants';

@Injectable({ providedIn: 'root' })
export class PageEnableDisableService {
  baseUrl = environment.apiUrl;

  constructor(
    private http: HttpClient,
    private router: Router) { }

  getCenters(): Observable<KeyValuePair[]> {
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'PageEnableDisable/Centers');
  }

  getTiers(center_Level: string): Observable<KeyValuePair[]> {
    const params = new HttpParams().appendAll({ center: center_Level });
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'PageEnableDisable/Tiers', { params });
  }

  getSections(center_Level: string, tier_Level: string): Observable<KeyValuePair[]> {
    const params = new HttpParams().appendAll({ center: center_Level, tier: tier_Level });
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'PageEnableDisable/Sections', { params });
  }

  getPages(param: PageEnableDisableParam): Observable<eTM_Page_Settings[]> {
    const params = new HttpParams().appendAll({ ...param });
    return this.http.get<eTM_Page_Settings[]>(this.baseUrl + 'PageEnableDisable/Pages', { params });
  }

  updatePages(pages: eTM_Page_Settings[]): Observable<OperationResult> {
    return this.http.put<OperationResult>(this.baseUrl + 'PageEnableDisable/Pages', pages);
  }

  async checkGuard(class_Level: string, dept_Id: string, nav: string, url: string) {
    let center_Level: string = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    let tier_Level: string = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    let param: PageEnableDisableParam = { center_Level, tier_Level, class_Level };
    let pages: eTM_Page_Settings[] = await this.getPages(param).toPromise();
    let pageIndex = pages.findIndex(item => url.includes(item.link));

    if (!center_Level || !tier_Level || !class_Level || pages.length === 0 || pages.every(item => !item.is_Active) || pageIndex === -1) {
      this.router.navigate(['/']);
      return false;
    }

    if (pages[pageIndex].is_Active) {
      return true;
    }

    pageIndex = nav === 'next' ? (pageIndex + 1 >= pages.length ? 0 : pageIndex + 1) : (pageIndex - 1 < 0 ? pages.length - 1 : pageIndex - 1);
    this.router.navigate([center_Level + "/" + tier_Level + pages[pageIndex].link, class_Level, dept_Id], { queryParams: { nav } });
    return false;
  }
}
