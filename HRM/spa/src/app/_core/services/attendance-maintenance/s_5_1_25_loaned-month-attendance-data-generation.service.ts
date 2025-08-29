import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { KeyValuePair } from '@utilities/key-value-pair';
import { HttpClient } from '@angular/common/http';
import {
  LoanedDataGeneration_Param,
  LoanedDataGeneration_Memory,
  LoanedDataGeneration_Base
} from '@models/attendance-maintenance/5_1_25_loaned-month-attendance-data-generation';
import { OperationResult } from '@utilities/operation-result';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_1_25_LoanedMonthAttendanceDataGenerationService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + 'C_5_1_25_LoanedMonthAttendanceDataGeneration';
  initData: LoanedDataGeneration_Memory = <LoanedDataGeneration_Memory>{
    data: <LoanedDataGeneration_Base>{
      param: <LoanedDataGeneration_Param>{
        close_Status: 'Y',
        selected_Tab: 'dataGeneration'
      }
    }
  }
  paramSearch = signal<LoanedDataGeneration_Memory>(structuredClone(this.initData))
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: LoanedDataGeneration_Memory) => this.paramSearch.set(data)

  constructor(private _http: HttpClient) { }
  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }

  getListFactory() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}/GetListFactory`, { params: { language: this.language } });
  }

  execute(params: LoanedDataGeneration_Param) {
    return this._http.post<OperationResult>(`${this.apiUrl}/Execute`, params);
  }
}
