import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { EmergencyContactsReportParam, EmergencyContactsReportSource } from '@models/employee-maintenance/4_2_2_emergency-contacts-report';
import { toObservable } from '@angular/core/rxjs-interop';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_4_2_2_EmergencyContactsReportService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl: string = environment.apiUrl + "C_4_2_2_EmergencyContactsSheetReport/"

  initData: EmergencyContactsReportSource = <EmergencyContactsReportSource>{
    param: <EmergencyContactsReportParam>{ employmentStatus: '' },
    totalRows: 0
  }
  programSource = signal<EmergencyContactsReportSource>(JSON.parse(JSON.stringify(this.initData)));
  programSource$ = toObservable(this.programSource);
  setSource = (source: EmergencyContactsReportSource) => this.programSource.set(source);
  clearParams = () => {
    this.programSource.set(JSON.parse(JSON.stringify(this.initData)))
  }

  constructor(private http: HttpClient) { }

  getTotalRows(param: EmergencyContactsReportParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<number>(this.apiUrl + 'GetTotalRows', { params })
  }
  downloadExcel(param: EmergencyContactsReportParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadExcel', { params });
  }
  getListDivision() {
    let params = new HttpParams().appendAll({ lang: this.language })
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDivision', { params });
  }
  getListFactory(division: string) {
    let params = new HttpParams().appendAll({ division, lang: this.language })
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params });
  }
  getListDepartment(division: string, factory: string) {
    let params = new HttpParams().appendAll({ division, factory, lang: this.language })
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params });
  }

}
