import { Injectable, signal } from '@angular/core';
import {
  BankAccountMaintenance_Basic,
  BankAccountMaintenanceDto,
  BankAccountMaintenanceParam
} from '@models/salary-maintenance/7_1_4_bank_account_maintenance';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { environment } from '@env/environment';
import { toObservable } from '@angular/core/rxjs-interop';
import { HttpClient, HttpParams } from '@angular/common/http';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_4_Bank_Account_MaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_7_1_4_BankAccountMaintenance/`;
  //signal
  initData: BankAccountMaintenance_Basic = <BankAccountMaintenance_Basic>{
    param: <BankAccountMaintenanceParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <BankAccountMaintenanceDto>{},
    data: []
  }
  paramSource = signal<BankAccountMaintenance_Basic>(structuredClone(this.initData));
  source$ = toObservable(this.paramSource);
  setSource = (data: BankAccountMaintenance_Basic) => this.paramSource.set(data)
  clearParams = () => {
    this.paramSource.set(structuredClone(this.initData))
  }
  constructor(private http: HttpClient) { }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }

  getData(pagination: PaginationParam, param: BankAccountMaintenanceParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<BankAccountMaintenanceDto>>(this.apiUrl + 'GetData', { params })
  }
  create(data: BankAccountMaintenanceDto) {
    return this.http.post<OperationResult>(this.apiUrl + "Create", data);
  }

  update(data: BankAccountMaintenanceDto) {
    return this.http.put<OperationResult>(this.apiUrl + "Update", data);
  }

  delete(data: BankAccountMaintenanceDto) {
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { body: data });
  }

  download(param: BankAccountMaintenanceParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadFileExcel', { params })
  }
  downloadTemplate() {
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadFileTemplate')
  }

  upload(file: FormData) {
    file.append('language', this.language);
    return this.http.post<OperationResult>(this.apiUrl + 'UploadFileExcel', file, {});
  }
}
