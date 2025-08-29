import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { HRMS_Emp_Identity_Card_HistoryDto, HRMS_Emp_Identity_Card_HistoryParam, HRMS_Emp_Identity_Card_History_Source, IdentificationCardHistory } from '@models/employee-maintenance/4_1_6_identification-card-history';
import { OperationResult } from '@utilities/operation-result';
import { environment } from '@env/environment';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_4_1_6_IdentificationCardHistoryService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + 'C_4_1_6_IdentificationCardHistory/';
  initDataSearch: IdentificationCardHistory = <IdentificationCardHistory>{
    param: <HRMS_Emp_Identity_Card_HistoryParam>{},
    data: []
  }
  paramSearch = signal<IdentificationCardHistory>(JSON.parse(JSON.stringify(this.initDataSearch)));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: IdentificationCardHistory) => this.paramSearch.set(data)

  initDataCodeSource: HRMS_Emp_Identity_Card_History_Source = <HRMS_Emp_Identity_Card_History_Source>{}
  basicCodeSource = signal<HRMS_Emp_Identity_Card_History_Source>(JSON.parse(JSON.stringify(this.initDataCodeSource)));
  basicCodeSource$ = toObservable(this.basicCodeSource);
  setSource = (source: HRMS_Emp_Identity_Card_History_Source) => this.basicCodeSource.set(source);

  constructor(private http: HttpClient) { }

  clearParams() {
    this.paramSearch.set(JSON.parse(JSON.stringify(this.initDataSearch)))
    this.basicCodeSource.set(JSON.parse(JSON.stringify(this.initDataCodeSource)))
  }
  getData(param: HRMS_Emp_Identity_Card_HistoryParam) {
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<HRMS_Emp_Identity_Card_HistoryDto[]>(this.apiUrl + "getData", { params })
  }
  create(param: HRMS_Emp_Identity_Card_HistoryDto) {
    return this.http.post<OperationResult>(this.apiUrl + "Create", param);
  }
  getListNationality() {
    return this.http.get<KeyValuePair[]>(
      this.apiUrl + 'GetListNationality', { params: { language: this.language } }
    );
  }

  getListTypeHeadIdentificationNumber(nationality: string) {
    return this.http.get<string[]>(
      this.apiUrl + 'GetListTypeHeadIdentificationNumber', { params: { nationality } }
    );
  }
}
