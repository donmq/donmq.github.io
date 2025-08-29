import { KeyValuePair } from '../../utilities/key-value-pair';
import { HttpParams, HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import {
  ChildcareSubsidyGenerationParam,
  ChildcareSubsidyGenerationSourceTab1,
  ChildcareSubsidyGenerationSourceTab2
} from '@models/salary-maintenance/7_1_15_childcare-subsidy-generation';
import { IClearCache } from '@services/cache.service';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_15_ChildcareSubsidyGenerationService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_7_1_15_ChildcareSubsidyGeneration/`;

  tabDefault: string = 'childSubsidyIsGenerated';
  tabSelected = signal<string>(structuredClone(this.tabDefault));
  tabSelected$ = toObservable(this.tabSelected);
  setTabSelected = (data: string) => this.tabSelected.set(data)

  initData1: ChildcareSubsidyGenerationSourceTab1 = <ChildcareSubsidyGenerationSourceTab1>{
    param: <ChildcareSubsidyGenerationParam>{
      kind_Tab1: "O",
      permissionGroupMultiple: [],
      lang: this.language
    },
    totalRows: 0,
    year_Month: null,
    dateFrom: null,
    dateTo: null,
  }
  programSource1 = signal<ChildcareSubsidyGenerationSourceTab1>(structuredClone(this.initData1));
  programSource1$ = toObservable(this.programSource1);
  setSource = (source: ChildcareSubsidyGenerationSourceTab1) => this.programSource1.set(source);

  //SourceTab2
  initData2: ChildcareSubsidyGenerationSourceTab2 = <ChildcareSubsidyGenerationSourceTab2>{
    param: <ChildcareSubsidyGenerationParam>{
      kind_Tab2: "S",
      permissionGroupMultiple: [],
      lang: this.language
    },
    totalRows: 0,
    year_Month: null
  }
  programSource2 = signal<ChildcareSubsidyGenerationSourceTab2>(structuredClone(this.initData2));
  programSource2$ = toObservable(this.programSource2);
  setSource1 = (source: ChildcareSubsidyGenerationSourceTab2) => this.programSource2.set(source);

  clearParams = () => {
    this.programSource1.set(structuredClone(this.initData1))
    this.programSource2.set(structuredClone(this.initData2))
    this.setTabSelected(this.tabDefault);
  }

  constructor(private _http: HttpClient) { }

  getListFactoryByUser() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetListFactoryByUser`, { params: { language: this.language } });
  }

  getListPermissionGroupByFactory(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}GetListPermissionGroupByFactory`, { params });
  }

  checkParam(data: ChildcareSubsidyGenerationParam) {
    let param: ChildcareSubsidyGenerationParam = <ChildcareSubsidyGenerationParam>{
      factory: data.factory,
      yearMonth: data.yearMonth,
      kind_Tab1: data.kind_Tab1,
      resignedDate_Start: data.resignedDate_Start,
      resignedDate_End: data.resignedDate_End,
      permissionGroupMultiple: data.permissionGroupMultiple,
    }
    let params = new HttpParams().appendAll({ ...param });
    return this._http.get<OperationResult>(`${this.baseUrl}CheckParam`, { params });
  }

  getTotalTab2(param: ChildcareSubsidyGenerationParam) {
    param.lang = this.language
    return this._http.get<number>(`${this.baseUrl}GetTotalTab2`, { params: { ...param } });
  }

  downloadExcel(param: ChildcareSubsidyGenerationParam) {
    param.lang = this.language
    return this._http.get<OperationResult>(`${this.baseUrl}DownloadExcel`, { params: { ...param } });
  }

  insertData(param: ChildcareSubsidyGenerationParam) {
    return this._http.get<OperationResult>(`${this.baseUrl}InsertData`, { params: { ...param } });
  }

}
