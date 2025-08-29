import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';
import { OperationResult } from '@utilities/operation-result';
import {
  MonthlySalaryGenerationExitedEmployees_Memory,
  MonthlySalaryGenerationExitedEmployees_Param,
} from '@models/salary-maintenance/7_1_23_monthly-salary-generation-exited-employees';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root',
})

export class S_7_1_23_MonthlySalaryGenerationExitedEmployees  implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_7_1_23_MonthlySalaryGenerationExitedEmployees/`;
  initData: MonthlySalaryGenerationExitedEmployees_Memory = <MonthlySalaryGenerationExitedEmployees_Memory>{
    salaryGeneration_Param: <MonthlySalaryGenerationExitedEmployees_Param>{
      total_Rows: 0,
      tab_Type: 'salaryGeneration'
    },
    dataLock_Param: <MonthlySalaryGenerationExitedEmployees_Param>{
      total_Rows: 0,
      tab_Type: 'dataLock'
    },
    selected_Tab: 'salaryGeneration'
  }
  paramSearch = signal<MonthlySalaryGenerationExitedEmployees_Memory>(structuredClone(this.initData))
  paramSearch$ = toObservable(this.paramSearch);

  setParamSearch = (data: MonthlySalaryGenerationExitedEmployees_Memory) => this.paramSearch.set(data)
  clearParams() {
    this.paramSearch.set(structuredClone(this.initData))
  }
  constructor(
    private http: HttpClient
  ) { }

  getDropDownList(param: MonthlySalaryGenerationExitedEmployees_Param) {
    param.lang = localStorage.getItem(LocalStorageConstants.LANG);
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  checkCloseStatus(param: MonthlySalaryGenerationExitedEmployees_Param) {
    return this.http.get<OperationResult>(`${this.baseUrl}CheckCloseStatus`, { params: { ...param } });
  }
  execute(data: MonthlySalaryGenerationExitedEmployees_Param): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.baseUrl}Execute`, data);
  }
}
