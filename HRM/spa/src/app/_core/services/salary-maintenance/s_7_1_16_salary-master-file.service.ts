import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { SalaryMasterFile_Detail, SalaryMasterFile_Main, SalaryMasterFile_Main_Memory, SalaryMasterFile_Param } from '@models/salary-maintenance/7_1_16_salary-master-file';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';


@Injectable({
  providedIn: 'root'
})
export class S_7_1_16_SalaryMasterFileService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_7_1_16_SalaryMasterFile/`;
  initDataMain: SalaryMasterFile_Main_Memory = <SalaryMasterFile_Main_Memory>{
    data: [],
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    paramSearch: <SalaryMasterFile_Param>{},
    selectedData: <SalaryMasterFile_Main>{}
  }
  signalDataMain = signal<SalaryMasterFile_Main_Memory>(structuredClone(this.initDataMain));
  signalDataMain$ = toObservable(this.signalDataMain);

  constructor(private _http: HttpClient) { }

  clearParams = () => {
    this.signalDataMain.set(structuredClone(this.initDataMain));
  }

  getDataPagination(pagination: Pagination, param: SalaryMasterFile_Param) {
    param.language = this.language
    let params = new HttpParams().appendAll({ "pageNumber": pagination.pageNumber, "pageSize": pagination.pageSize, ...param })
    return this._http.get<PaginationResult<SalaryMasterFile_Main>>(`${this.apiUrl}GetDataPagination`, { params })
  }

  getListFactory() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListFactory`, { params: { language: this.language } });
  }

  getDepartments(factory: string) {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetDepartments`, { params: { factory, language: this.language } });
  }

  getPositionTitles() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetPositionTitles`, { params: { language: this.language } });
  }

  getSalaryTypes() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetSalaryTypes`, { params: { language: this.language } });
  }

  getListPermissionGroup() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListPermissionGroup`, { params: { language: this.language } });
  }

  getDataQueryPage(pagination: Pagination, factory: string, employee_ID: string,) {
    let params = new HttpParams().appendAll({
      "pageNumber": pagination.pageNumber,
      "pageSize": pagination.pageSize,
      "factory": factory,
      "employee_ID": employee_ID,
      "language": this.language
    });
    return this._http.get<SalaryMasterFile_Detail>(`${this.apiUrl}GetDataQueryPage`, { params });
  }

  getTechnicalTypes() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetTechnicalTypes`, { params: { language: this.language } });
  }

  getExpertiseCategorys() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetExpertiseCategorys`, { params: { language: this.language } });
  }

  download(param: SalaryMasterFile_Param) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this._http.get<OperationResult>(this.apiUrl + 'DownloadFileExcel', { params })
  }
}
