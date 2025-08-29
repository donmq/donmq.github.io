import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { SalaryApprovalForm_Param, SalaryApprovalForm_Source } from '@models/salary-report/7_2_1_SalaryApprovalForm';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class S_7_2_1_SalaryApprovalFormService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  //apiUrl = `${environment.apiUrl} D_7_2_1_SalaryApprovalForm`;
  apiUrl: string = environment.apiUrl + "C_7_2_1_SalaryApprovalForm/"
  initData: SalaryApprovalForm_Source = <SalaryApprovalForm_Source>{
    param: <SalaryApprovalForm_Param>{
      kind: 'O',
      permission_Group: []
    },
    totalRows: 0
  }

  programSource = signal<SalaryApprovalForm_Source>(structuredClone(this.initData));
  programSource$ = toObservable(this.programSource);
  setSource = (source: SalaryApprovalForm_Source) => this.programSource.set(source);
  clearParams = () => {
    this.programSource.set(structuredClone(this.initData))
  }
  constructor(private _http: HttpClient) { }

  search(param: SalaryApprovalForm_Param) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this._http.get<number>(this.apiUrl + 'GetSearch', { params })
  }

  download(param: SalaryApprovalForm_Param) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this._http.get<OperationResult>(this.apiUrl + 'ExportPDF', { params });
  }
  getListPermissionGroup(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this._http.get<KeyValuePair[]>(this.apiUrl + 'GetListPermissionGroup', { params });
  }
  getListFactory() {
    let params = new HttpParams().appendAll({language: this.language })
    return this._http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params });
  }
  getListDepartment(factory: string) {
    let params = new HttpParams().appendAll({ factory, language: this.language })
    return this._http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params });
  }
  getPositionTitles() {
    let params = new HttpParams().appendAll({ language: this.language })
    return this._http.get<KeyValuePair[]>(this.apiUrl + 'GetPositionTitles', { params });
  }
}
