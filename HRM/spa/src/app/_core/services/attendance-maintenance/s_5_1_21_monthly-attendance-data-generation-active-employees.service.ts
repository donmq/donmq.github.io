import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import {
  ActiveMonthlyDataCloseParam,
  GenerationActiveParam,
  SearchAlreadyDeadlineDataMain,
  SearchAlreadyDeadlineDataParam,
  SearchAlreadyDeadlineDataSource
} from '@models/attendance-maintenance/5_1_21_monthly-attendance-data-generation-active-employees';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root',
})
export class S_5_1_21_MonthlyAttendanceDataGenerationActiveEmployeesService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + 'C_5_1_21_MonthlyAttendanceDataGenerationActiveEmployees/';

  tabDefault: string = 'dataGeneration';
  tabSelected = signal<string>(structuredClone(this.tabDefault));
  tabSelected$ = toObservable(this.tabSelected);
  setTabSelected = (data: string) => this.tabSelected.set(data)

  initData1: GenerationActiveParam = <GenerationActiveParam>{}
  programSource1 = signal<GenerationActiveParam>(structuredClone(this.initData1));
  programSource1$ = toObservable(this.programSource1);
  setSource1 = (data: GenerationActiveParam) => this.programSource1.set(data)

  initData2: SearchAlreadyDeadlineDataSource = <SearchAlreadyDeadlineDataSource>{
    param: {},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0,
    },
    data: [],
  }
  programSource2 = signal<SearchAlreadyDeadlineDataSource>(structuredClone(this.initData2));
  programSource2$ = toObservable(this.programSource2);
  setSource2 = (data: SearchAlreadyDeadlineDataSource) => this.programSource2.set(data)

  initData3: ActiveMonthlyDataCloseParam = <ActiveMonthlyDataCloseParam>{}
  programSource3 = signal<ActiveMonthlyDataCloseParam>(structuredClone(this.initData3));
  programSource3$ = toObservable(this.programSource3);
  setSource3 = (data: ActiveMonthlyDataCloseParam) => this.programSource3.set(data)

  clearParams = () => {
    this.programSource1.set(structuredClone(this.initData1))
    this.programSource2.set(structuredClone(this.initData2))
    this.programSource3.set(structuredClone(this.initData3))
    this.setTabSelected(this.tabDefault);
  }

  constructor(private _http: HttpClient) { }

  checkParam(param: GenerationActiveParam) {
    return this._http.get<OperationResult>(`${this.apiUrl}CheckParam`, { params: { ...param } });
  }

  monthlyAttendanceDataGeneration(param: GenerationActiveParam) {
    return this._http.put<OperationResult>(`${this.apiUrl}MonthlyAttendanceDataGeneration`, param);
  }

  searchAlreadyDeadlineData(pagination: Pagination, param: SearchAlreadyDeadlineDataParam) {
    return this._http.get<PaginationResult<SearchAlreadyDeadlineDataMain>>(`${this.apiUrl}SearchAlreadyDeadlineData`, { params: { ...pagination, ...param } });
  }

  monthlyDataClose(param: ActiveMonthlyDataCloseParam) {
    return this._http.put<OperationResult>(`${this.apiUrl}MonthlyDataCloseExecute`, param);
  }

  getListFactoryAdd() {
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}GetListFactoryAdd`, { params: { language: this.language } });
  }

  getListDepartment(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this._http.get<KeyValuePair[]>(`${this.apiUrl}QueryDepartmentList`, { params });
  }
}
