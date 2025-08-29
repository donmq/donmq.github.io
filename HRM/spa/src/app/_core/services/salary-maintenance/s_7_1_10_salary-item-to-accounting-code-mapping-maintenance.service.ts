import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { toObservable } from '@angular/core/rxjs-interop'
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import {
  SalaryItemToAccountingCodeMappingMaintenanceDto,
  SalaryItemToAccountingCodeMappingMaintenanceParam,
  SalaryItemToAccountingCodeMappingMaintenance_Main_Memory
} from '@models/salary-maintenance/7_1_10_salary-item-to-accounting-code-mapping-maintenance';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';


@Injectable({
  providedIn: 'root'
})
export class S_7_1_10_SalaryItemToAccountingCodeMappingMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = environment.apiUrl + "C_7_1_10_SalaryItemToAccountingCodeMappingMaintenance/";
  initDataMain: SalaryItemToAccountingCodeMappingMaintenance_Main_Memory = <SalaryItemToAccountingCodeMappingMaintenance_Main_Memory>{
    data: [],
    selectedData: <SalaryItemToAccountingCodeMappingMaintenanceDto>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    param: <SalaryItemToAccountingCodeMappingMaintenanceParam>{}
  }
  signalDataMain = signal<SalaryItemToAccountingCodeMappingMaintenance_Main_Memory>(structuredClone(this.initDataMain));
  signalDataMain$ = toObservable(this.signalDataMain);

  constructor(private http: HttpClient) { }

  clearParams = () => {
    this.signalDataMain.set(structuredClone(this.initDataMain));
  }

  getDataPagination(pagination: Pagination, param: SalaryItemToAccountingCodeMappingMaintenanceParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ 'pageNumber': pagination.pageNumber, 'pageSize': pagination.pageSize, ...param });
    return this.http.get<PaginationResult<SalaryItemToAccountingCodeMappingMaintenanceDto>>(this.apiUrl + "GetDataPagination", { params });
  }

  getFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + "GetFactory", { params: { language: this.language } });
  }

  create(model: SalaryItemToAccountingCodeMappingMaintenanceDto) {
    return this.http.post<OperationResult>(this.apiUrl + "Create", model);
  }

  edit(model: SalaryItemToAccountingCodeMappingMaintenanceDto) {
    return this.http.put<OperationResult>(this.apiUrl + "Edit", model);
  }

  delete(factory: string, salary_Item: string, dC_Code: string) {
    let params = new HttpParams().appendAll({ 'factory': factory, 'salary_Item': salary_Item, 'dC_Code': dC_Code });
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { params });
  }

  checkDupplicate(factory: string, salary_Item: string, dC_Code: string) {
    let params = new HttpParams()
      .append("factory", factory)
      .append("salary_Item", salary_Item)
      .append("dC_Code", dC_Code);
    return this.http.get<OperationResult>(this.apiUrl + "CheckDupplicate", { params });
  }

}
