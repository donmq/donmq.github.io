import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from '@env/environment';
import { EmployeeBasicInformationReportParam } from '@models/employee-maintenance/4_2_1_employee-basic-information-report';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_4_2_1_EmployeeBasicInformationReportService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl: string = environment.apiUrl + 'C_4_2_1_EmployeeBasicInformationReport';
  initData: EmployeeBasicInformationReportParam = <EmployeeBasicInformationReportParam>{
    employmentStatus: 'all',
  }
  baseInitSource = signal<EmployeeBasicInformationReportParam>(JSON.parse(JSON.stringify(this.initData)));
  setSource = (source: EmployeeBasicInformationReportParam) => this.baseInitSource.set(source);
  clearParams = () => { this.baseInitSource.set(JSON.parse(JSON.stringify(this.initData))) }
  constructor(private http: HttpClient) { }

  getPagination(param: EmployeeBasicInformationReportParam) {
    param.language = this.language
    var params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(`${this.baseUrl}/GetData`, { params });
  }

  export(param: EmployeeBasicInformationReportParam) {
    param.language = this.language
    var params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(`${this.baseUrl}/Export`, { params });
  }

  getListNationality() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}/GetListNationality`, { params: { language: this.language } }
    );
  }

  getListDivision() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}/GetListDivision`, { params: { language: this.language } }
    );
  }

  getListFactory(division: string) {
    var params = new HttpParams().appendAll({ division, language: this.language });
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}/GetListFactory`, { params }
    );
  }

  getListDepartment(division: string, factory: string) {
    var params = new HttpParams().appendAll({ division, factory, language: this.language });
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}/GetListDepartment`, { params }
    );
  }

  getListPermission() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}/GetListPermission`, { params: { language: this.language } }
    );
  }
  getListPositonGrade() {
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}/GetListPositonGrade`);
  }
}
