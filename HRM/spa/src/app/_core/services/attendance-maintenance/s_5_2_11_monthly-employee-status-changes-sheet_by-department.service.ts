import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import {
  MonthlyEmployeeStatus_ByDepartmentParam,
  MonthlyEmployeeStatus_ByDepartmentSource
} from '@models/attendance-maintenance/5_2_11_monthly-employee-status-changes-sheet_by-department';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_2_11_monthlyEmployeeStatusChangesSheet_byDepartmentService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + "C_5_2_11_MonthlyEmployeeStatusChangesSheet_ByDepartment/"

  initData: MonthlyEmployeeStatus_ByDepartmentSource = <MonthlyEmployeeStatus_ByDepartmentSource>{
    param: <MonthlyEmployeeStatus_ByDepartmentParam>{
      permissionGroup: [],
      permissionName: []
    },
    totalRows: 0
  }
  programSource = signal<MonthlyEmployeeStatus_ByDepartmentSource>(structuredClone(this.initData));
  programSource$ = toObservable(this.programSource);
  setSource = (source: MonthlyEmployeeStatus_ByDepartmentSource) => this.programSource.set(source);
  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  }

  constructor(private http: HttpClient) { }
  getTotalRows(param: MonthlyEmployeeStatus_ByDepartmentParam) {
    param.lang = this.language;
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<number>(this.apiUrl + 'GetTotalRows', { params })
  }
  downloadExcel(param: MonthlyEmployeeStatus_ByDepartmentParam) {
    param.lang = this.language;
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadExcel', { params });
  }
  getListFactory() {
    let params = new HttpParams().appendAll({ lang: this.language })
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params });
  }
  getListLevel() {
    let params = new HttpParams().appendAll({ lang: this.language })
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListLevel', { params });
  }
  getListPermissionGroup(factory: string) {
    let params = new HttpParams().appendAll({ factory, lang: this.language })
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListPermissionGroup', { params });
  }

}
