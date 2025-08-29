import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import { AnnualLeaveCalculationParam, AnnualLeaveCalculationSource } from "@models/attendance-maintenance/5_2_22_annual-leave-calculation";
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_2_22_AnnualLeaveCalculationService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + "C_5_2_22_AnnualLeaveCalculation/"

  initData: AnnualLeaveCalculationSource = <AnnualLeaveCalculationSource>{
    param: <AnnualLeaveCalculationParam>{
      permission_Group: [],
      start_Year_Month: new Date().toFirstDateOfYear().toStringYearMonth(),
      end_Year_Month: new Date().toStringYearMonth(),
      kind: 'O'
    },
    totalRows: 0
  }
  programSource = signal<AnnualLeaveCalculationSource>(structuredClone(this.initData));
  programSource$ = toObservable(this.programSource);
  setSource = (source: AnnualLeaveCalculationSource) => this.programSource.set(source);
  clearParams() {
    this.programSource.set(structuredClone(this.initData))
  }

  constructor(private http: HttpClient) { }

  getTotalRows(param: AnnualLeaveCalculationParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<number>(this.apiUrl + 'GetTotalRows', { params })
  }

  download(param: AnnualLeaveCalculationParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'Download', { params });
  }

  getListFactory() {
    let params = new HttpParams().appendAll( { language: this.language })
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params });
  }

  getListDepartment(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params });
  }

  getListPermissionGroup(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListPermissionGroup', { params });
  }
}
