import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { BatchUpdateData_Param, FinSalaryCloseMaintenance_MainData, FinSalaryCloseMaintenance_Memory, FinSalaryCloseMaintenance_Param, FinSalaryCloseMaintenance_UpdateParam } from '@models/salary-maintenance/7_1_26_fin-salary-close-maintenance';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam } from '@utilities/pagination-utility';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_26_FinSalaryCloseMaintenanceService implements IClearCache {

  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiURL: string = environment.apiUrl + 'C_7_1_26_FinSalaryCloseMaintenance/';

  initData: FinSalaryCloseMaintenance_Memory = <FinSalaryCloseMaintenance_Memory>{
    salaryCloseSearch_Param: <FinSalaryCloseMaintenance_Param>{
      kind: "O"
    },
    batchUpdateData_Param: <BatchUpdateData_Param>{
      kind: "O",
      close_Status: "Y"
    },
    salaryCloseSearch_Data: [],
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <FinSalaryCloseMaintenance_MainData>{},
    selectedTab: 'salaryCloseSearch'
  }

  programSource = signal<FinSalaryCloseMaintenance_Memory>(structuredClone(this.initData));
  programSource$ = toObservable(this.programSource);
  setSource = (data: FinSalaryCloseMaintenance_Memory) => this.programSource.set(data);

  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  };
  constructor(private _http: HttpClient) { }

  getListFactory() {
    return this._http.get<KeyValuePair[]>(`${this.apiURL}GetListFactory`, { params: { language: this.language } });
  }

  GetDepartment(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this._http.get<KeyValuePair[]>(`${this.apiURL}GetDepartment`, { params })
  }

  getListPermissionGroup(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this._http.get<KeyValuePair[]>(`${this.apiURL}GetListPermissionGroup`, { params });
  }
  getListTypeHeadEmployeeID(factory: string) {
    return this._http.get<string[]>(`${this.apiURL}GetListTypeHeadEmployeeID`, { params: { factory } })
  }
  getData(pagination: PaginationParam, param: FinSalaryCloseMaintenance_Param) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this._http.get<OperationResult>(`${this.apiURL}GetDataPaination`, { params })
  }

  update(data: FinSalaryCloseMaintenance_MainData) {
    return this._http.put<OperationResult>(`${this.apiURL}Update`, data)
  }
  download(param: FinSalaryCloseMaintenance_Param) {
    return this._http.get<OperationResult>(`${this.apiURL}DownloadExcel`, { params: { ...param } })
  }
  excute(param: BatchUpdateData_Param) {
    return this._http.put<OperationResult>(`${this.apiURL}BatchUpdateData`, param)
  }
}
