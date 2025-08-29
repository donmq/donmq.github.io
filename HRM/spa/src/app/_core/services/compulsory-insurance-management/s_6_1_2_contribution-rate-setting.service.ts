import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable, signal } from "@angular/core";
import { toObservable } from "@angular/core/rxjs-interop";
import { LocalStorageConstants } from "@constants/local-storage.constants";
import { environment } from "@env/environment";
import { ContributionRateSettingCheckEffectiveMonth, ContributionRateSettingDto, ContributionRateSettingForm, ContributionRateSettingParam, ContributionRateSettingSource, ContributionRateSettingSubData, ContributionRateSettingSubParam } from "@models/compulsory-insurance-management/6_1_2_contribution-rate-setting";
import { IClearCache } from "@services/cache.service";
import { KeyValuePair } from "@utilities/key-value-pair";
import { OperationResult } from "@utilities/operation-result";
import { Pagination, PaginationParam, PaginationResult } from "@utilities/pagination-utility";


@Injectable({
  providedIn: 'root'
})
export class S_6_1_2_ContributionRateSettingService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl: string = environment.apiUrl + "C_6_1_2_ContributionRateSetting/";

  initData: ContributionRateSettingSource = <ContributionRateSettingSource>{
    source: <ContributionRateSettingDto>{},
    paramQuery: <ContributionRateSettingParam>{},
    dataMain: [],
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
  }

  programSource = signal<ContributionRateSettingSource>(structuredClone(this.initData));
  source = toObservable(this.programSource);
  setSource = (source: ContributionRateSettingSource) => this.programSource.set(source);
  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  }

  constructor(private http: HttpClient) { }

  getData(pagination: PaginationParam, param: ContributionRateSettingParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<ContributionRateSettingDto>>(this.baseUrl + 'GetData', { params });
  }
  getDetail(param: ContributionRateSettingSubParam) {
    let params = new HttpParams().appendAll({ ...JSON.parse(JSON.stringify(param)) })
    return this.http.get<ContributionRateSettingSubData[]>(this.baseUrl + 'GetDetail', { params });
  }
  create(data: ContributionRateSettingForm) {
    return this.http.post<OperationResult>(this.baseUrl + "Create", data);
  }
  update(data: ContributionRateSettingForm) {
    return this.http.put<OperationResult>(this.baseUrl + "Update", data);
  }
  delete(data: ContributionRateSettingDto) {
    return this.http.delete<OperationResult>(this.baseUrl + "Delete", { params: {}, body: data });
  }
  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListFactory", { params: { language : this.language  } });
  }
  getListPermissionGroup(factory: string) {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListPermissionGroup", { params: { factory, language : this.language } });
  }
  getListInsuranceType() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListInsuranceType", { params: { language : this.language  } });
  }
  checkSearch(param: ContributionRateSettingParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<boolean>(this.baseUrl + "CheckSearch", { params });
  }
  checkEffectiveMonth(param: ContributionRateSettingSubParam) {
    const params = new HttpParams().appendAll({
      ...param,
      effective_Month: param.effective_Month.toISOString()
    });
    return this.http.get<ContributionRateSettingCheckEffectiveMonth>(this.baseUrl + "CheckEffectiveMonth", { params });
  }
}
