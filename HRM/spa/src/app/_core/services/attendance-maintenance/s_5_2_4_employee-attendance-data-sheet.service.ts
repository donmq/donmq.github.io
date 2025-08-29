import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { toObservable } from '@angular/core/rxjs-interop';
import { OperationResult } from '@utilities/operation-result';
import {
  EmployeeAttendanceDataSheet_Basic,
  EmployeeAttendanceDataSheetParam
} from '@models/attendance-maintenance/5_2_4_employee-attendance-data-sheet';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_2_4_EmployeeAttendanceDataSheetService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_5_2_4_EmployeeAttendanceDataSheet/`;

  //signal
  initData: EmployeeAttendanceDataSheet_Basic = <EmployeeAttendanceDataSheet_Basic>{
    param: <EmployeeAttendanceDataSheetParam>{},
    data: null,
  }

  paramSource = signal<EmployeeAttendanceDataSheet_Basic>(structuredClone(this.initData));
  source$ = toObservable(this.paramSource);
  setSource = (data: EmployeeAttendanceDataSheet_Basic) => this.paramSource.set(data)
  clearParams = () => {
    this.paramSource.set(structuredClone(this.initData))
  }
  constructor(private http: HttpClient) { }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }
  getListWorkShiftType() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListWorkShiftType', { params: { language: this.language } });
  }
  getListDepartment(factory: string, ) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params: { factory, language: this.language } });
  }

  download(param: EmployeeAttendanceDataSheetParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadFileExcel', { params })
  }

  getCountRecords(param: EmployeeAttendanceDataSheetParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<number>(this.apiUrl + 'GetCountRecords', { params })
  }
}
