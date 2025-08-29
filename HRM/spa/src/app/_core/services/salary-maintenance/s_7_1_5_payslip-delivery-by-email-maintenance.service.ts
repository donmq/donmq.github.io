import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import {
  PayslipDeliveryByEmailMaintenanceDto,
  PayslipDeliveryByEmailMaintenanceParam,
  PayslipDeliveryByEmailMaintenanceSource
} from '@models/salary-maintenance/7_1_5_payslip-delivery-by-email-maintenance';
import { toObservable } from '@angular/core/rxjs-interop';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_5_PayslipDeliveryByEmailMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  constructor(private http: HttpClient) { }
  apiUrl = `${environment.apiUrl}C_7_1_5_PayslipDeliveryByEmailMaintenance/`;

  initData = <PayslipDeliveryByEmailMaintenanceSource>{
    param: <PayslipDeliveryByEmailMaintenanceParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <PayslipDeliveryByEmailMaintenanceDto>{},
    data: []
  };
  paramSearch = signal<PayslipDeliveryByEmailMaintenanceSource>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: PayslipDeliveryByEmailMaintenanceSource) => this.paramSearch.set(data)

  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }

  checkDuplicate(factory: string, employeeID: string) {
    return this.http.get<OperationResult>(this.apiUrl + "CheckDuplicate", { params: { factory, employeeID } });
  }
  getData(pagination: PaginationParam, param: PayslipDeliveryByEmailMaintenanceParam) {
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<PayslipDeliveryByEmailMaintenanceDto>>(this.apiUrl + "GetData", { params })
  }

  downloadExcel(param: PayslipDeliveryByEmailMaintenanceParam, isTemplate: boolean) {
    return this.http.get<OperationResult>(this.apiUrl + "DownloadExcel", { params: { ...param, isTemplate } });
  }

  uploadExcel(file: FormData) {
    return this.http.post<OperationResult>(this.apiUrl + 'UploadExcel', file)
  }

  add(dataList: PayslipDeliveryByEmailMaintenanceDto) {
    return this.http.post<OperationResult>(this.apiUrl + "AddNew", dataList);
  }

  edit(data: PayslipDeliveryByEmailMaintenanceDto) {
    return this.http.put<OperationResult>(this.apiUrl + "Edit", data)
  }

  delete(data: PayslipDeliveryByEmailMaintenanceDto) {
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { params: {}, body: data });
  }
}

