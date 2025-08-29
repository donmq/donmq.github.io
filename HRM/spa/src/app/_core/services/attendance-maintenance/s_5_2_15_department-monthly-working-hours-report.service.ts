import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import {
  DepartmentMonthlyWorkingHoursReportMemory,
  DepartmentMonthlyWorkingHoursReportParam
} from '@models/attendance-maintenance/5_2_15_department-monthly-working-hours-report';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_2_15_DepartmentMonthlyWorkingHoursReportService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  private apiUrl: string = environment.apiUrl + "C_5_2_15_DepartmentMonthlyWorkingHoursReport/";
  initData: DepartmentMonthlyWorkingHoursReportMemory = <DepartmentMonthlyWorkingHoursReportMemory>{
    param: <DepartmentMonthlyWorkingHoursReportParam>{},
    totalRows: 0
  }
  signalDataMain = signal<DepartmentMonthlyWorkingHoursReportMemory>(JSON.parse(JSON.stringify(this.initData)));
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

  getData(param: DepartmentMonthlyWorkingHoursReportParam) {
    param.language = this.language
    return this.http.get<OperationResult>(this.apiUrl + "GetData", { params: { ...param } });
  }

  excel(param: DepartmentMonthlyWorkingHoursReportParam) {
    param.language = this.language;
    return this.http.get<OperationResult>(this.apiUrl + "Excel", { params: { ...param } });
  }


}
