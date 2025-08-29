import {
  INodeDto,
  OrganizationChartParam,
  OrganizationChart_MainMemory
} from '@models/organization-management/3_1_3_organization-chart';
import { environment } from '@env/environment';
import { KeyValuePair } from '@utilities/key-value-pair';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { OperationResult } from '@utilities/operation-result';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root',
})

export class S_3_1_3_OrganizationChart implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_3_1_3_OrganizationChart/`;
  initData: OrganizationChart_MainMemory = <OrganizationChart_MainMemory>{
    param: <OrganizationChartParam>{ level: '7' },
    position: null,
    data: []
  }
  paramSearch = signal<OrganizationChart_MainMemory>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: OrganizationChart_MainMemory) => this.paramSearch.set(data)
  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  constructor(private http: HttpClient) { }

  getDropDownList(division?: string) {
    let param: OrganizationChartParam = <OrganizationChartParam>{
      lang: localStorage.getItem(LocalStorageConstants.LANG)
    }
    if (division)
      param.division = division
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  getDepartmentList(param: OrganizationChartParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDepartmentList`, { params });
  }
  getChartData(param: OrganizationChartParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<INodeDto[]>(`${this.baseUrl}GetChartData`, {
      params,
    });
  }
  downloadExcel(param: OrganizationChartParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(` ${this.baseUrl}DownloadExcel`, { params });
  }
}

export const OrganizationChartResolver: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_3_1_3_OrganizationChart).getDropDownList();
};
