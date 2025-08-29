import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { environment } from '@env/environment';
import { inject, Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { toObservable } from '@angular/core/rxjs-interop'
import {
  FinSalaryAttributionCategoryMaintenance_Data,
  FinSalaryAttributionCategoryMaintenance_Param,
  FinSalaryAttributionCategoryMaintenance_Memory,
  FinSalaryAttributionCategoryMaintenance_Update
} from '@models/salary-maintenance/7_1_27_fin-salary-attribution-category-maintenance';
import { Pagination } from '@utilities/pagination-utility';
import { ResolveFn } from '@angular/router';
import { Observable } from 'rxjs';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_27_FinSalaryAttributionCategoryMaintenance implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_7_1_27_FinSalaryAttributionCategoryMaintenance/`;
  initData: FinSalaryAttributionCategoryMaintenance_Memory = <FinSalaryAttributionCategoryMaintenance_Memory>{
    param: <FinSalaryAttributionCategoryMaintenance_Param>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<FinSalaryAttributionCategoryMaintenance_Memory>(structuredClone(this.initData))
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: FinSalaryAttributionCategoryMaintenance_Memory) => this.paramSearch.set(data)

  constructor(private http: HttpClient) { }

  clearParams() {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getSearchDetail(param: Pagination, filter: FinSalaryAttributionCategoryMaintenance_Param): Observable<OperationResult> {
    filter.lang = this.language
    let params = new HttpParams().appendAll({ ...param, ...filter });
    return this.http.get<OperationResult>(`${this.baseUrl}GetSearch`, { params });
  }
  getDropDownList() {
    let param: FinSalaryAttributionCategoryMaintenance_Param = <FinSalaryAttributionCategoryMaintenance_Param>{
      lang: this.language
    }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  getDepartmentList(param: FinSalaryAttributionCategoryMaintenance_Param) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDepartmentList`, { params });
  }
  getKindCodeList(param: FinSalaryAttributionCategoryMaintenance_Param) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetKindCodeList`, { params });
  }
  isExistedData(factory: string, kind: string, department: string, kind_Code: string) {
    const data = <FinSalaryAttributionCategoryMaintenance_Data>{
      factory: factory,
      kind: kind,
      department: department,
      kind_Code: kind_Code,
    }
    let params = new HttpParams().appendAll({ ...data });
    return this.http.get<boolean>(` ${this.baseUrl}IsExistedData`, { params });
  }
  putData(param: FinSalaryAttributionCategoryMaintenance_Data): Observable<OperationResult> {
    return this.http.put<OperationResult>(`${this.baseUrl}PutData`, param);
  }
  deleteData(data: FinSalaryAttributionCategoryMaintenance_Data): Observable<OperationResult> {
    return this.http.delete<OperationResult>(`${this.baseUrl}DeleteData`, { params: {}, body: data });
  }
  postData(param: FinSalaryAttributionCategoryMaintenance_Param, data: FinSalaryAttributionCategoryMaintenance_Data[]): Observable<OperationResult> {
    const inputData: FinSalaryAttributionCategoryMaintenance_Update = <FinSalaryAttributionCategoryMaintenance_Update>{
      param: param,
      data: data
    }
    return this.http.post<OperationResult>(`${this.baseUrl}PostData`, inputData);
  }
  downloadExcelTemplate() {
    return this.http.get<OperationResult>(` ${this.baseUrl}DownloadExcelTemplate`);
  }
  downloadExcel(param: FinSalaryAttributionCategoryMaintenance_Param) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(` ${this.baseUrl}DownloadExcel`, { params });
  }
  uploadExcel(file: FormData) {
    return this.http.post<OperationResult>(`${this.baseUrl}UploadExcel`, file);
  }
}
export const FinSalaryAttributionCategoryMaintenanceResolver: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_7_1_27_FinSalaryAttributionCategoryMaintenance).getDropDownList();
};
