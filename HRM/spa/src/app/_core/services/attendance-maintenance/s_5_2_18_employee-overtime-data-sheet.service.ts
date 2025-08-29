import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { HttpClient, HttpParams } from '@angular/common/http';
import { EmployeeOvertimeDataSheetParam } from '@models/attendance-maintenance/5_2_18_Employee-overtime-data-sheet';
import { environment } from '@env/environment';
import { Injectable, signal } from '@angular/core';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_2_18_EmployeeOvertimeDataSheetService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl: string = environment.apiUrl + 'C_5_2_18_EmployeeOvertimeDataSheet';

  initData: EmployeeOvertimeDataSheetParam = <EmployeeOvertimeDataSheetParam>{}
  baseInitSource = signal<EmployeeOvertimeDataSheetParam>(structuredClone(this.initData));
  setSource = (source: EmployeeOvertimeDataSheetParam) => this.baseInitSource.set(source);
  clearParams = () => { this.baseInitSource.set(structuredClone(this.initData)) }
  constructor(private http: HttpClient) { }

  getPagination(param: EmployeeOvertimeDataSheetParam) {
    param.language = this.language
    var params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(`${this.baseUrl}/GetData`, { params });
  }

  export(param: EmployeeOvertimeDataSheetParam) {
    param.language = this.language
    var params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(`${this.baseUrl}/Export`, { params });
  }

  getListDepartment(factory: string) {
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}/GetListDepartment`, { params: { factory, language: this.language } });
  }

  getFactories() {
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}/GetFactories`, { params: { language: this.language } })
  }


}
