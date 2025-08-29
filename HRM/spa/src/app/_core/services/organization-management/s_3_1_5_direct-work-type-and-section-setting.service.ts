import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@env/environment';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import {
  DirectWorkTypeAndSectionSetting,
  DirectWorkTypeAndSectionSettingParam,
  HRMS_Org_Direct_SectionDto
} from '@models/organization-management/3_1_5_organization-management';
import { OperationResult } from '@utilities/operation-result';
import { toObservable } from '@angular/core/rxjs-interop'
import { BehaviorSubject } from 'rxjs';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LocalStorageConstants } from '@constants/local-storage.constants';
@Injectable({
  providedIn: 'root'
})
export class S_3_1_5_DirectWorkTypeAndSectionSettingService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = `${environment.apiUrl}C_3_1_5_DirectWorkTypeAndSectionSetting/`;

  initData: DirectWorkTypeAndSectionSetting = <DirectWorkTypeAndSectionSetting>{
    param: <DirectWorkTypeAndSectionSettingParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <HRMS_Org_Direct_SectionDto>{ direct_Section: 'Y' },
    data: []
  }
  paramSearch = signal<DirectWorkTypeAndSectionSetting>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: DirectWorkTypeAndSectionSetting) => this.paramSearch.set(data)

  paramSearchSource = new BehaviorSubject<DirectWorkTypeAndSectionSettingParam>(null);
  paramSearchSource$ = this.paramSearchSource.asObservable();
  changeParamSearch(paramsearch: DirectWorkTypeAndSectionSettingParam) {
    paramsearch.lang = this.language
    this.paramSearchSource.next(paramsearch);
  }

  constructor(private http: HttpClient) { }

  clearParams() {
    this.paramSearch.set(structuredClone(this.initData))
    this.paramSearchSource.next(null)
  }

  getData(pagination: PaginationParam, param: DirectWorkTypeAndSectionSettingParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<HRMS_Org_Direct_SectionDto>>(this.apiUrl + "GetData", { params })
  }

  create(param: DirectWorkTypeAndSectionSettingParam) {
    param.lang = this.language
    return this.http.post<OperationResult>(this.apiUrl + "Create", param);
  }

  update(param: DirectWorkTypeAndSectionSettingParam) {
    param.lang = this.language
    return this.http.put<OperationResult>(this.apiUrl + "Update", param);
  }

  download(param: DirectWorkTypeAndSectionSettingParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadExcel', { params })
  }

  getListDivision() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDivision', { params: { language: this.language } });
  }
  getListFactory(division: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { division, language: this.language } });
  }
  getListWorkType() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListWorkType', { params: { language: this.language } });
  }
  getListSection() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListSection', { params: { language: this.language } });
  }
  checkDuplicate(param: DirectWorkTypeAndSectionSettingParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'CheckDuplicate', { params })
  }
}
