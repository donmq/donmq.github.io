import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { MonthlyDataLockParam, MonthlySalaryGeneration_Memory, MonthlySalaryGenerationParam } from '@models/salary-maintenance/7_1_22_monthly-salary-generation';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_22_MonthlySalaryGenerationService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + 'C_7_1_22_MonthlySalaryGeneration/';

  initData: MonthlySalaryGeneration_Memory = <MonthlySalaryGeneration_Memory>{
    salaryGeneration_Param: <MonthlySalaryGenerationParam>{ totalRows: 0 },
    dataLock_Param: <MonthlyDataLockParam>{ totalRows: 0, salary_Lock: "Y" },
    selectedTab: 'salaryGeneration'
  }
  programSource = signal<MonthlySalaryGeneration_Memory>(structuredClone(this.initData));
  programSource$ = toObservable(this.programSource);
  setSource = (data: MonthlySalaryGeneration_Memory) => this.programSource.set(data)



  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  }

  constructor(private _http: HttpClient) { }

  checkData(param: MonthlySalaryGenerationParam) {
    return this._http.get<OperationResult>(`${this.apiUrl}CheckData`, { params: { ...param } });
  }

  monthlySalaryGenerationExecute(param: MonthlySalaryGenerationParam) {
    return this._http.put<OperationResult>(`${this.apiUrl}MonthlySalaryGenerationExecute`, param);
  }

  monthlyDataLockExecute(param: MonthlyDataLockParam) {
    return this._http.put<OperationResult>(`${this.apiUrl}MonthlyDataLockExecute`, param);
  }

  getListFactory() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListFactory`, { params: { language: this.language } });
  }

  getListPermissionGroup(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListPermissionGroup`, { params });
  }
}
