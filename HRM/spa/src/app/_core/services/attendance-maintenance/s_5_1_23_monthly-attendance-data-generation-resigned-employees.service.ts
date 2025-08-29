import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { KeyValuePair } from '@utilities/key-value-pair';
import { HttpClient, HttpParams } from '@angular/common/http';
import { OperationResult } from '@utilities/operation-result';
import {
  GenerationResigned,
  GenerationResignedParam,
  ResignedMonthlyDataCloseParam
} from '@models/attendance-maintenance/5_1_23_monthly-attendance-data-generation-resigned-employees';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root',
})
export class S_5_1_23_MonthlyAttendanceDataGenerationResignedEmployeesService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + 'C_5_1_23_MonthlyAttendanceDataGenerationResignedEmployees/';

  tabDefault: string = 'dataGeneration';
  tabSelected = signal<string>(structuredClone(this.tabDefault));
  tabSelected$ = toObservable(this.tabSelected);
  setTabSelected = (data: string) => this.tabSelected.set(data)

  initData1: GenerationResigned = <GenerationResigned>{}

  programSource1 = signal<GenerationResigned>(structuredClone(this.initData1));
  programSource1$ = toObservable(this.programSource1);
  setSource1 = (data: GenerationResigned) => this.programSource1.set(data)

  initData2: ResignedMonthlyDataCloseParam = <ResignedMonthlyDataCloseParam>{}

  programSource2 = signal<ResignedMonthlyDataCloseParam>(structuredClone(this.initData2));
  programSource2$ = toObservable(this.programSource2);
  setSource2 = (data: ResignedMonthlyDataCloseParam) => this.programSource2.set(data)

  clearParams = () => {
    this.programSource1.set(structuredClone(this.initData1))
    this.programSource2.set(structuredClone(this.initData2))
    this.setTabSelected(this.tabDefault);
  }

  constructor(private _http: HttpClient) { }

  checkParam(data: GenerationResigned) {
    let param: GenerationResignedParam = <GenerationResignedParam>{
      factory: data.factory,
      att_Month: data.att_Month,
      resign_Date: data.resign_Date,
      employee_ID_Start: data.employee_ID_Start,
      employee_ID_End: data.employee_ID_End
    }
    let params = new HttpParams().appendAll({ ...param });
    return this._http.get<OperationResult>(`${this.apiUrl}CheckParam`, { params });
  }

  monthlyAttendanceDataGeneration(param: GenerationResigned) {
    return this._http.put<OperationResult>(`${this.apiUrl}MonthlyAttendanceDataGeneration`, param);
  }

  monthlyDataClose(param: ResignedMonthlyDataCloseParam) {
    return this._http.put<OperationResult>(`${this.apiUrl}MonthlyDataCloseExecute`, param);
  }

  getListFactoryAdd() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListFactoryAdd`, {
      params: { language: this.language },
    });
  }

  getListDepartment(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language });
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}QueryDepartmentList`, {
      params,
    });
  }
}
