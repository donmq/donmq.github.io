import { Injectable, signal } from '@angular/core';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import {
  DeleteParam,
  ExistedDataParam,
  SAPCostCenterSettingDto,
  SAPCostCenterSettingParam,
  SAPCostCenterSettingSource
} from '@models/salary-maintenance/7_1_8_sap-cost-center-setting';
import { IClearCache } from '@services/cache.service';
@Injectable({
  providedIn: 'root'
})
export class S_7_1_8_SapCostCenterSettingService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  constructor(private http: HttpClient) { }
  apiUrl = `${environment.apiUrl}C_7_1_8_SapCostCenterSetting/`;
  apiCommonUrl = `${environment.apiUrl}Common/`;
  initData = <SAPCostCenterSettingSource>(<SAPCostCenterSettingSource>{
    param: <SAPCostCenterSettingParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <SAPCostCenterSettingDto>{},
    data: []
  });

  paramSearch = signal<SAPCostCenterSettingSource>(structuredClone(this.initData))
  setSource = (data: SAPCostCenterSettingSource) => this.paramSearch.set(data)
  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getData(pagination: PaginationParam, param: SAPCostCenterSettingParam) {
    param.language = this.language
    const params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<SAPCostCenterSettingDto>>(this.apiUrl + "Search", { params })
  }

  addNew(param: SAPCostCenterSettingDto) {
    return this.http.post<OperationResult>(`${this.apiUrl}Create`, param);
  }

  edit(param: SAPCostCenterSettingDto) {
    return this.http.put<OperationResult>(this.apiUrl + "Update", param);
  }

  delete(param: DeleteParam) {
    return this.http.post<OperationResult>(this.apiUrl + "Delete", param);
  }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}GetListFactory`, { params: { language: this.language } });
  }

  getListKind() {
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}GetListKind`, { params: { language: this.language } });
  }

  checkExistedData(data: ExistedDataParam) {
    const params = new HttpParams()
      .set('cost_Year', data.cost_Year)
      .set('company_Code', data.company_Code)
      .set('cost_Code', data.cost_Code)
    return this.http.get<OperationResult>(`${this.apiUrl}CheckExistedData`, { params });
  }

  checkExistedDataCompanyCode(factory: string, companyCode: string) {
    const params = new HttpParams()
      .set('factory', factory)
      .set('companyCode', companyCode)
    return this.http.get<OperationResult>(`${this.apiUrl}CheckExistedDataCompanyCode`, { params });
  }

  checkExistedCostCenter(data: ExistedDataParam) {
    const params = new HttpParams()
      .set('cost_Year', data.cost_Year)
      .set('company_Code', data.company_Code)
      .set('cost_Code', data.cost_Code)
    return this.http.get<OperationResult>(`${this.apiUrl}CheckExistedCostCenter`, { params });
  }

  getListFactoryByUser() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactoryByUser', { params: { language: this.language } });
  }

  downloadExcel() {
    return this.http.get<OperationResult>(this.apiUrl + "DownloadExcelTemplate");
  }

  excelExport(param: SAPCostCenterSettingParam) {
    param.language = this.language
    const params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'ExcelExport', { params });
  }

  uploadExcel(file: FormData) {
    return this.http.post<OperationResult>(this.apiUrl + 'UploadExcel', file)
  }

}
