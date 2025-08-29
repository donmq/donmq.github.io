import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@env/environment';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { toObservable } from '@angular/core/rxjs-interop';
import {
  IndividualMonthlyWorkingHoursReportMemory,
  IndividualMonthlyWorkingHoursReportParam
} from '@models/attendance-maintenance/5_2_16_individual-monthly-working-hours-report';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_2_16_IndividualMonthlyWorkingHoursReportService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  private apiUrl: string = environment.apiUrl + "C_5_2_16_IndividualMonthlyWorkingHoursReport/";
  initData: IndividualMonthlyWorkingHoursReportMemory = <IndividualMonthlyWorkingHoursReportMemory>{
    param: <IndividualMonthlyWorkingHoursReportParam>{},
    totalRows: 0
  }
  signalDataMain = signal<IndividualMonthlyWorkingHoursReportMemory>(JSON.parse(JSON.stringify(this.initData)));
  signalDataMain$ = toObservable(this.signalDataMain);

  constructor(private http: HttpClient) { }

  clearParams = () => {
    this.signalDataMain.set(JSON.parse(JSON.stringify(this.initData)));
  }

  getFactorys() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + "GetListFactory", { params: { language: this.language } });
  }

  getPermissionGroups(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + "GetListPermissionGroup", { params: { factory, language: this.language } });
  }

  getData(param: IndividualMonthlyWorkingHoursReportParam) {
    param.language = this.language
    return this.http.get<OperationResult>(this.apiUrl + "GetData", { params: { ...param } });
  }

  excel(param: IndividualMonthlyWorkingHoursReportParam) {
    param.language = this.language
    return this.http.get<OperationResult>(this.apiUrl + "Excel", { params: { ...param } });
  }

}
