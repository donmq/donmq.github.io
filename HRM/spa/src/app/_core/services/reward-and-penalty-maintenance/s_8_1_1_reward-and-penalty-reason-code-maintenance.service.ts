import { Injectable, signal } from '@angular/core';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IClearCache } from '@services/cache.service';
import { RewardandPenaltyMaintenance, RewardandPenaltyMaintenance_Basic, RewardandPenaltyMaintenance_Form, RewardandPenaltyMaintenanceParam } from '@models/reward-and-penalty-maintenance/8_1_1_reward-and-penalty-reason-code-maintenance';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { toObservable } from '@angular/core/rxjs-interop';
import { KeyValuePair } from '@utilities/key-value-pair';
import { environment } from '@env/environment';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class S_8_1_1_RewardAndPenaltyReasonCodeMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_8_1_1_RewardAndPenaltyReasonCodeMaintenance/`;

  initData: RewardandPenaltyMaintenance_Basic = <RewardandPenaltyMaintenance_Basic>{
    param: <RewardandPenaltyMaintenanceParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    source: <RewardandPenaltyMaintenance>{},
    data: []
  }
  paramSource = signal<RewardandPenaltyMaintenance_Basic>(structuredClone(this.initData));
  source$ = toObservable(this.paramSource);
  setSource = (data: RewardandPenaltyMaintenance_Basic) => this.paramSource.set(data)
  clearParams = () => {
    this.paramSource.set(structuredClone(this.initData))
  }
  constructor(private http: HttpClient) { }

  getData(pagination: PaginationParam, param: RewardandPenaltyMaintenanceParam) {
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<RewardandPenaltyMaintenance>>(this.apiUrl + 'GetData', { params })
  }
  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }
  getListReason(factory : string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListReason',{ params: { factory } });
  }
  create(data: RewardandPenaltyMaintenance_Form) {
    return this.http.post<OperationResult>(this.apiUrl + "Create", data);
  }

  update(data: RewardandPenaltyMaintenance) {
    return this.http.put<OperationResult>(this.apiUrl + "Update", data);
  }

  delete(data: RewardandPenaltyMaintenance) {
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { body: data });
  }

  download(param: RewardandPenaltyMaintenanceParam) {
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadFileExcel', { params })
  }

  isDuplicatedData(data: RewardandPenaltyMaintenance) {
    let params = new HttpParams().appendAll({ ...data });
    return this.http.get<OperationResult>(this.apiUrl + 'IsDuplicatedData', { params });
  }
}
