import { Injectable, signal } from '@angular/core';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { UserForLogged } from '@models/auth/auth';
import {
  CardSwipingDataFormatSettingMain,
  HRMS_Att_Swipecard_SetDto,
  CardSwipingDataFormatSettingSource
} from '@models/attendance-maintenance/5_1_8_hrms_att_swipecard_set'
import { KeyValuePair } from '@utilities/key-value-pair';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
@Injectable({
  providedIn: 'root'
})
export class S_5_1_8_CardSwipingDataFormatSettingService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  user: UserForLogged = JSON.parse((localStorage.getItem(LocalStorageConstants.USER)));
  constructor(private http: HttpClient) { }
  apiUrl = `${environment.apiUrl}C_5_1_8_CardSwipingDataFormatSetting/`;
  initData: CardSwipingDataFormatSettingSource = <CardSwipingDataFormatSettingSource>{
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSource = signal<CardSwipingDataFormatSettingSource>(structuredClone(this.initData));
  paramSource$ = toObservable(this.paramSource);
  setSource = (data: CardSwipingDataFormatSettingSource) => this.paramSource.set(data)
  clearParams = () => {
    this.paramSource.set(structuredClone(this.initData))
  }
  getData(pagination: PaginationParam, factory: string) {
    let params = new HttpParams().appendAll({ ...pagination, factory });
    return this.http.get<PaginationResult<CardSwipingDataFormatSettingMain>>(this.apiUrl + "GetData", { params })
  }

  addNew(param: HRMS_Att_Swipecard_SetDto) {
    return this.http.post<OperationResult>(`${this.apiUrl}AddNew`, param);
  }

  edit(param: HRMS_Att_Swipecard_SetDto) {
    return this.http.put<OperationResult>(this.apiUrl + "Edit", param)
  }

  getFactoryMain() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetFactoryMain', { params: { language: this.language } });
  }

  getByFactoryAddList() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetByFactoryAddList', { params: { language: this.language } });
  }

  getByFactory(factory: string) {
    return this.http.get<HRMS_Att_Swipecard_SetDto>(this.apiUrl + 'GetDataByFactory', { params: { factory: factory } });
  }

}
