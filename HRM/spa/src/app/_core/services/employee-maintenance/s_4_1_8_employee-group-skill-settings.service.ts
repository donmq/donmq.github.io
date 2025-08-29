import { EmployeeGroupSkillSettings_Main, EmployeeGroupSkillSettings_MainMemory, EmployeeGroupSkillSettings_Param } from '@models/employee-maintenance/4_1_8_employee-group-skill-settings';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { ResolveFn } from '@angular/router';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { Observable, BehaviorSubject } from 'rxjs';
import { OperationResult } from '@utilities/operation-result';
import { environment } from '@env/environment';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root',
})

export class S_4_1_8_EmployeeGroupSkillSettings implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_4_1_8_EmployeeGroupSkillSettings/`;
  initData: EmployeeGroupSkillSettings_MainMemory = <EmployeeGroupSkillSettings_MainMemory>{
    param: <EmployeeGroupSkillSettings_Param>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<EmployeeGroupSkillSettings_MainMemory>(JSON.parse(JSON.stringify(this.initData)));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: EmployeeGroupSkillSettings_MainMemory) => this.paramSearch.set(data)

  paramForm = new BehaviorSubject<EmployeeGroupSkillSettings_Main>(null);
  paramForm$ = this.paramForm.asObservable();
  setParamForm = (item: EmployeeGroupSkillSettings_Main) => this.paramForm.next(item);

  constructor(private http: HttpClient) { }

  clearParams() {
    this.paramSearch.set(JSON.parse(JSON.stringify(this.initData)))
    this.paramForm.next(null)
  }
  getDropDownList(division?: string) {
    let param: EmployeeGroupSkillSettings_Param = <EmployeeGroupSkillSettings_Param>{ lang: this.language }
    if (division)
      param.division = division
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetDropDownList`, { params });
  }
  getSearchDetail(param: Pagination, filter: EmployeeGroupSkillSettings_Param): Observable<PaginationResult<EmployeeGroupSkillSettings_Main>> {
    filter.lang = this.language
    let params = new HttpParams().appendAll({ ...param, ...filter });
    return this.http.get<PaginationResult<EmployeeGroupSkillSettings_Main>>(
      `${this.baseUrl}GetSearchDetail`, { params }
    );
  }
  getEmployeeList(factory: string, employee_Id: string) {
    let param: EmployeeGroupSkillSettings_Param = <EmployeeGroupSkillSettings_Param>{
      factory: factory,
      employee_Id: employee_Id
    }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<KeyValuePair[]>(` ${this.baseUrl}GetEmployeeList`, { params });
  }
  checkExistedData(division: string, factory: string, employee_Id: string) {
    let param: EmployeeGroupSkillSettings_Param = <EmployeeGroupSkillSettings_Param>{
      division: division,
      factory: factory,
      employee_Id: employee_Id
    }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(` ${this.baseUrl}CheckExistedData`, { params });
  }
  postData(data: EmployeeGroupSkillSettings_Main): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.baseUrl}PostData`, data);
  }
  putData(data: EmployeeGroupSkillSettings_Main): Observable<OperationResult> {
    return this.http.put<OperationResult>(`${this.baseUrl}PutData`, data);
  }
  deleteData(division: string, factory: string, employee_Id: string) {
    let params = new HttpParams().appendAll({ division, factory, employee_Id })
    return this.http.delete<OperationResult>(`${this.baseUrl}DeleteData`, { params });
  }
}
export const employeeGroupSkillResolver: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_4_1_8_EmployeeGroupSkillSettings).getDropDownList();
};
