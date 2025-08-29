import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import {
  RehireEvaluationForFormerEmployees,
  RehireEvaluationForFormerEmployeesDto,
  RehireEvaluationForFormerEmployeesEvaluation,
  RehireEvaluationForFormerEmployeesParam,
  RehireEvaluationForFormerEmployeesPersonal,
  RehireEvaluationForFormerEmployeesSource
} from '@models/employee-maintenance/4_1_18_rehire-evaluation-for-former-employees';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { OperationResult } from '@utilities/operation-result';
import { toObservable } from '@angular/core/rxjs-interop';
import { BehaviorSubject } from 'rxjs';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_4_1_18_RehireEvaluationForFormerEmployeesService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + "C_4_1_18_RehireEvaluationForFormerEmployees/"

  initData: RehireEvaluationForFormerEmployees = <RehireEvaluationForFormerEmployees>{
    param: <RehireEvaluationForFormerEmployeesParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: []
  }
  paramSearch = signal<RehireEvaluationForFormerEmployees>(JSON.parse(JSON.stringify(this.initData)));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: RehireEvaluationForFormerEmployees) => this.paramSearch.set(data)

  initDataCodeSource: RehireEvaluationForFormerEmployeesSource = <RehireEvaluationForFormerEmployeesSource>{
    formType: '',
    data: <RehireEvaluationForFormerEmployeesDto>{
      personal: <RehireEvaluationForFormerEmployeesPersonal>{},
      evaluation: <RehireEvaluationForFormerEmployeesEvaluation>{}
    }
  }
  basicCodeSource = signal<RehireEvaluationForFormerEmployeesSource>(JSON.parse(JSON.stringify(this.initDataCodeSource)));
  basicCodeSource$ = toObservable(this.basicCodeSource);
  setSource = (source: RehireEvaluationForFormerEmployeesSource) => this.basicCodeSource.set(source);

  paramSearchSource = new BehaviorSubject<RehireEvaluationForFormerEmployeesParam>(null);
  paramSearchSource$ = this.paramSearchSource.asObservable();
  changeParamSearch(paramsearch: RehireEvaluationForFormerEmployeesParam) { this.paramSearchSource.next(paramsearch); }

  constructor(private http: HttpClient) { }

  clearParams() {
    this.paramSearch.set(JSON.parse(JSON.stringify(this.initData)))
    this.basicCodeSource.set(JSON.parse(JSON.stringify(this.initDataCodeSource)))
    this.paramSearchSource.next(null)
  }
  getData(pagination: PaginationParam, param: RehireEvaluationForFormerEmployeesParam) {
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<RehireEvaluationForFormerEmployeesDto>>(this.apiUrl + 'GetData', { params })
  }
  create(param: RehireEvaluationForFormerEmployeesEvaluation) {
    return this.http.post<OperationResult>(this.apiUrl + "Create", param);
  }
  update(param: RehireEvaluationForFormerEmployeesEvaluation) {
    return this.http.put<OperationResult>(this.apiUrl + "Update", param);
  }
  getDetail(nationality: string, identification_Number: string) {
    return this.http.get<RehireEvaluationForFormerEmployeesPersonal>(
      this.apiUrl + "GetDetail", { params: { nationality, identification_Number } }
    );
  }

  getListResignationType() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListResignationType', { params: { language: this.language } }
    );
  }

  getListReasonforResignation() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListReasonforResignation', { params: { language: this.language } }
    );
  }
  getListDivision() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListDivision', { params: { language: this.language } }
    );
  }
  getListFactory(division: string) {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListFactory', { params: { language: this.language, division } }
    );
  }
  getListDepartment(factory: string, division: string) {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListDepartment', { params: { language: this.language, factory, division } }
    );
  }
  getListTypeHeadIdentificationNumber(nationality: string) {
    return this.http.get<string[]>(
      this.apiUrl + 'GetListTypeHeadIdentificationNumber', { params: { nationality } }
    );
  }

}
