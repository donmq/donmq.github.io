import { Injectable, signal } from '@angular/core';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { Observable } from 'rxjs';
import {
  DeleteParam,
  ExistedDataParam,
  HRMS_Sal_Childcare_SubsidyDto,
  ListofChildcareSubsidyRecipientsMaintenanceParam,
  ListofChildcareSubsidyRecipientsMaintenanceSource
} from '@models/salary-maintenance/7_1_7_list-of-child-care-subsidy-recipients-maintenance';
import { EmployeeInfo } from '@models/attendance-maintenance/5_1_18_attendance-change-record-maintenance';
import { IClearCache } from '@services/cache.service';
@Injectable({
  providedIn: 'root'
})
export class S_7_1_7_ListofChildcareSubsidyRecipientsMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  constructor(private http: HttpClient) { }
  apiUrl = `${environment.apiUrl}C_7_1_7_ListofChildcareSubsidyRecipientsMaintenance/`;
  apiCommonUrl = `${environment.apiUrl}Common/`;
  initData = <ListofChildcareSubsidyRecipientsMaintenanceSource>(<ListofChildcareSubsidyRecipientsMaintenanceSource>{
    param: <ListofChildcareSubsidyRecipientsMaintenanceParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <HRMS_Sal_Childcare_SubsidyDto>{},
    data: []
  });

  paramSearch = signal<ListofChildcareSubsidyRecipientsMaintenanceSource>(structuredClone(this.initData))
  setSource = (data: ListofChildcareSubsidyRecipientsMaintenanceSource) => this.paramSearch.set(data)
  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getData(pagination: PaginationParam, param: ListofChildcareSubsidyRecipientsMaintenanceParam) {
    param.language = this.language
    const params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<HRMS_Sal_Childcare_SubsidyDto>>(this.apiUrl + "Search", { params })
  }

  addNew(param: HRMS_Sal_Childcare_SubsidyDto) {
    return this.http.post<OperationResult>(`${this.apiUrl}Create`, param);
  }

  edit(param: HRMS_Sal_Childcare_SubsidyDto) {
    return this.http.put<OperationResult>(this.apiUrl + "Update", param);
  }

  delete(param: DeleteParam) {
    return this.http.post<OperationResult>(this.apiUrl + "Delete", param);
  }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(`${this.apiUrl}GetListFactory`, { params: { language: this.language } });
  }

  checkExistedData(data: ExistedDataParam) {
    const params = new HttpParams()
      .set('Factory', data.factory)
      .set('Employee_ID', data.employee_ID)
      .set('Birthday_Child', data.birthday_Child as string)
    return this.http.get<OperationResult>(`${this.apiUrl}CheckExistedData`, { params });
  }

  getListFactoryByUser() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactoryByUser', { params: { language: this.language } });
  }

  downloadExcel() {
    return this.http.get<OperationResult>(this.apiUrl + "DownloadExcelTemplate");
  }

  excelExport(param: ListofChildcareSubsidyRecipientsMaintenanceParam) {
    param.language = this.language
    if (param.language == "zh") param.language = "tw"
    const params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'ExcelExport', { params });
  }

  uploadExcel(file: FormData) {
    return this.http.post<OperationResult>(this.apiUrl + 'UploadExcel', file)
  }

}
