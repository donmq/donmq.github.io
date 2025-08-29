import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import {
  Sal_Dept_SAPCostCenter_MappingParam,
  Sal_Dept_SAPCostCenter_MappingSource
} from '@models/salary-maintenance/7_1_9_sal_dept_sapcostcenter_mapping';
import { Sal_Dept_SAPCostCenter_MappingDTO } from '@models/salary-maintenance/7_1_9_sal_dept_sapcostcenter_mapping';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_9_departmentToSapCostCenterMappingService implements IClearCache {
  baseUrl: string = environment.apiUrl + "C_7_1_9_DepartmentToSAPCostCenterMappingMaintenance/";
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }

  initData: Sal_Dept_SAPCostCenter_MappingSource = <Sal_Dept_SAPCostCenter_MappingSource>{
    selectedData: <Sal_Dept_SAPCostCenter_MappingDTO>{},
    param: <Sal_Dept_SAPCostCenter_MappingParam>{},
    data: [],
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
  }

  programSource = signal<Sal_Dept_SAPCostCenter_MappingSource>(structuredClone(this.initData));
  source = toObservable(this.programSource);
  setSource = (source: Sal_Dept_SAPCostCenter_MappingSource) => this.programSource.set(source);
  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  }
  constructor(private http: HttpClient) { }

  getData(pagination: PaginationParam, param: Sal_Dept_SAPCostCenter_MappingParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<Sal_Dept_SAPCostCenter_MappingDTO>>(this.baseUrl + 'GetData', { params });
  }
  create(data: Sal_Dept_SAPCostCenter_MappingDTO) {
    return this.http.post<OperationResult>(this.baseUrl + "Create", data);
  }
  update(data: Sal_Dept_SAPCostCenter_MappingDTO) {
    return this.http.put<OperationResult>(this.baseUrl + "Update", data);
  }
  delete(data: Sal_Dept_SAPCostCenter_MappingDTO) {
    return this.http.delete<OperationResult>(this.baseUrl + "Delete", { params: {}, body: data });
  }
  downloadTemplate() {
    return this.http.get<OperationResult>(this.baseUrl + 'DownloadTemplate');
  }
  downloadExcel(param: Sal_Dept_SAPCostCenter_MappingParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.baseUrl + 'DownloadExcel', { params });
  }
  uploadExcel(file: FormData) {
    return this.http.post<OperationResult>(this.baseUrl + 'UploadExcel', file);
  }
  getListFactory() {
    let params = new HttpParams().appendAll({ lang: this.language })
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'GetListFactory', { params });
  }
  getListCostCenter(param: Sal_Dept_SAPCostCenter_MappingParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param })
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'GetListCostCenter', { params });
  }
  getListDepartment(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'GetListDepartment', { params });
  }
  checkDuplicate(factory: string, year: string, department: string) {
    let params = new HttpParams().appendAll({ factory, year, department })
    return this.http.get<boolean>(this.baseUrl + 'CheckDuplicate', { params });
  }

}
