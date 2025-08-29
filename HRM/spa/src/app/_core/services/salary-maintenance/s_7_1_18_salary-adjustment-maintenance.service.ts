import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { KeyValuePair } from '@utilities/key-value-pair';
import { CheckEffectiveDateResult, HistoryDetail, SalaryAdjustmentMaintenance_PersonalDetail, SalaryAdjustmentMaintenanceMain, SalaryAdjustmentMaintenanceParam, SalaryAdjustmentMaintenanceSource, SalaryAdjustmentMaintenance_SalaryItem } from '@models/salary-maintenance/7_1_18_salary-adjustment-maintenance';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_18_salaryAdjustmentMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl: string = environment.apiUrl + "C_7_1_18_SalaryAdjustmentMaintenance/";
  initData: SalaryAdjustmentMaintenanceSource = <SalaryAdjustmentMaintenanceSource>{
    selectedData: <SalaryAdjustmentMaintenanceMain>{},
    paramQuery: <SalaryAdjustmentMaintenanceParam>{},
    dataMain: [],
    pagination: <Pagination>{},
  }
  programSource = signal<SalaryAdjustmentMaintenanceSource>(structuredClone(this.initData));
  source = toObservable(this.programSource);
  setSource = (source: SalaryAdjustmentMaintenanceSource) => this.programSource.set(source);
  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  }


  constructor(private http: HttpClient) { }
  getData(pagination: PaginationParam, param: SalaryAdjustmentMaintenanceParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<SalaryAdjustmentMaintenanceMain>>(this.baseUrl + 'GetDataPagination', { params });
  }
  create(data: SalaryAdjustmentMaintenanceMain) {
    return this.http.post<OperationResult>(this.baseUrl + "Create", data);
  }
  update(data: SalaryAdjustmentMaintenanceMain) {
    return this.http.put<OperationResult>(this.baseUrl + "Update", data);
  }
  downloadTemplate(factory: string) {
    return this.http.get<OperationResult>(this.baseUrl + 'DownloadTemplate', { params: { factory } });
  }
  downloadExcel(param: SalaryAdjustmentMaintenanceParam) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.baseUrl + 'DownloadExcel', { params });
  }
  uploadExcel(file: FormData) {
    return this.http.post<OperationResult>(this.baseUrl + 'UploadExcel', file);
  }
  getDetailPersonal(factory: string, employee_ID: string) {
    return this.http.get<SalaryAdjustmentMaintenance_PersonalDetail>(this.baseUrl + 'GetDetailPersonal', { params: { factory, employee_ID, language: this.language } })
  }
  checkEffectiveDate(factory: string, employee_ID: string, inputEffectiveDate: string) {
    return this.http.get<CheckEffectiveDateResult>(this.baseUrl + 'CheckEffectiveDate', { params: { factory, employee_ID, inputEffectiveDate } })
  }
  getlistEmployeeID(factory: string) {
    return this.http.get<string[]>(this.baseUrl + "GetListEmployeeID", { params: { factory } });
  }
  getlistFactory() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListFactory", { params: { language: this.language } });
  }
  getlistDepartment(factory: string,) {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListDepartment", { params: { factory, language: this.language } });
  }
  getlistReasonForChange() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListReason", { params: { language: this.language } });
  }
  getlistTechnicalType() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListTechnicalType", { params: { language: this.language } });
  }
  getlistExpertiseCategory() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListExpertiseCategory", { params: { language: this.language } });
  }
  getlistPermissionGroup() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListPermissionGroup", { params: { language: this.language } });
  }
  getlistSalaryType() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListSalaryType", { params: { language: this.language } });
  }
  getListSalaryItem(history_GUID: string,) {
    return this.http.get<SalaryAdjustmentMaintenance_SalaryItem[]>(this.baseUrl + "GetListSalaryItem", { params: { history_GUID, language: this.language } });
  }
  getlistPositionTitle() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListPositionTitle", { params: { language: this.language } });
  }
  getlistCurrency() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListCurrency", { params: { language: this.language } });
  }
  getSalaryItemsAsync(factory: string, permissionGroup: string, salaryType: string, type: string, employeeID: string) {
    return this.http.get<SalaryAdjustmentMaintenance_SalaryItem[]>(this.baseUrl + "GetSalaryItemsAsync", { params: { factory, permissionGroup, salaryType, type, language: this.language, employeeID } });
  }
  checkReasonForChange(reasonForChange: string) {
    return this.http.get<boolean>(this.baseUrl + 'CheckReasonForChange', { params: { reasonForChange } });
  }
}
