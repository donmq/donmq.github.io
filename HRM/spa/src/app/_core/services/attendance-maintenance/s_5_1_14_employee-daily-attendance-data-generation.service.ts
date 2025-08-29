import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { LangConstants } from '@constants/lang-constants';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { HRMS_Att_Swipe_Card_Excute_Param } from '@models/attendance-maintenance/5_1_14_employee-daily-attendance-data-generation';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root'
})
export class S_5_1_14_EmployeeDailyAttendanceDataGenerationService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_5_1_14_EmployeeDailyAttendanceDataGeneration`;
  constructor(private _http: HttpClient) { }
  clearParams: () => void;

  getFactories() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}/getFactories`, { params: { language: this.language } });
  }

  checkClockInDateInCurrentDate(factory: string, card_Date: string) {
    return this._http.get<OperationResult>(`${this.baseUrl}/checkClockInDateInCurrentDate`, { params: { factory, card_Date } });
  }

  getHolidays(factory: string, offWork: string, workDay: string) {
    return this._http.get<OperationResult>(`${this.baseUrl}/getHolidays`, { params: { factory, offWork, workDay } });
  }

  getNationalHolidays(factory: string, offWork: string, workDay: string) {
    return this._http.get<OperationResult>(`${this.baseUrl}/getNationalHolidays`, { params: { factory, offWork, workDay } });
  }

  excute(param: HRMS_Att_Swipe_Card_Excute_Param) {
    return this._http.post<OperationResult>(`${this.baseUrl}/excute`, param);
  }
}

export const resolverFactories: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_5_1_14_EmployeeDailyAttendanceDataGenerationService).getFactories();
};

