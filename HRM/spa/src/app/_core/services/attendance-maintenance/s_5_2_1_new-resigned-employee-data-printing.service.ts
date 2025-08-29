import { OperationResult } from '@utilities/operation-result';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import {
  NewResignedEmployeeDataPrintingParam,
  NewResignedEmployeeDataPrintingSource
} from '@models/attendance-maintenance/5_2_1_new-resigned-employee-data-printing';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_2_1_NewResignedEmployeeDataPrintingService implements IClearCache {

  constructor(private http: HttpClient) { }
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_5_2_1_NewResignedEmployeeDataPrinting/`;

  initData = <NewResignedEmployeeDataPrintingSource>{
    dateFrom: null,
    dateTo: null,
    selectedKey: 'NewHired',
    param: <NewResignedEmployeeDataPrintingParam>{
      factory: '',
      department: ''
    },
    total: 0
  };
  paramSearch = signal<NewResignedEmployeeDataPrintingSource>(structuredClone(this.initData));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: NewResignedEmployeeDataPrintingSource) => this.paramSearch.set(data)

  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }

  getListDepartment(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params: { factory, language: this.language } });
  }

  getTotal(param: NewResignedEmployeeDataPrintingParam) {
    param.lang = this.language
    return this.http.get<number>(this.apiUrl + 'GetTotal', { params: { ...param } })
  }

  downloadExcel(param: NewResignedEmployeeDataPrintingParam) {
    param.lang = this.language
    return this.http.get<OperationResult>(this.apiUrl + "DownloadExcel", { params: { ...param } });
  }
}
