import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import { OperationResult } from '@utilities/operation-result';
import {
  NewEmployeesCompulsoryInsurancePremium_Memory,
  NewEmployeesCompulsoryInsurancePremium_Param,
} from '@models/compulsory-insurance-management/6_1_4_new_employees_compulsory_insurance_premium';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root',
})

export class S_6_1_4_NewEmployeesCompulsoryInsurancePremium implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_6_1_4_NewEmployeesCompulsoryInsurancePremium/`;
  initData: NewEmployeesCompulsoryInsurancePremium_Memory = <NewEmployeesCompulsoryInsurancePremium_Memory>{
    generation_Param: <NewEmployeesCompulsoryInsurancePremium_Param>{
      total_Rows: 0
    },
    report_Param: <NewEmployeesCompulsoryInsurancePremium_Param>{
      total_Rows: 0
    },
    selected_Tab: 'generation'
  }
  paramSearch = signal<NewEmployeesCompulsoryInsurancePremium_Memory>(structuredClone(this.initData))
  paramSearch$ = toObservable(this.paramSearch);

  setParamSearch = (data: NewEmployeesCompulsoryInsurancePremium_Memory) => this.paramSearch.set(data)
  clearParams() {
    this.paramSearch.set(structuredClone(this.initData))
  }

  constructor(
    private http: HttpClient
  ) { }
  getDropDownList(param: NewEmployeesCompulsoryInsurancePremium_Param) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  process(param: NewEmployeesCompulsoryInsurancePremium_Param) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(`${this.baseUrl}Process`, { params });
  }
}
