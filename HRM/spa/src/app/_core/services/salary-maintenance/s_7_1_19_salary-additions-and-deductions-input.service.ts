import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { SalaryAdditionsAndDeductionsInput_Basic, SalaryAdditionsAndDeductionsInput_Param, SalaryAdditionsAndDeductionsInput_Personal, SalaryAdditionsAndDeductionsInputDto } from '@models/salary-maintenance/7_1_19_salary-additions-and-deductions-input';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';

@Injectable({
  providedIn: 'root'
})
export class S_7_1_19_SalaryAdditionsAndDeductionsInputService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_7_1_19_SalaryAdditionsAndDeductionsInput/`;
  //signal
  initData: SalaryAdditionsAndDeductionsInput_Basic = <SalaryAdditionsAndDeductionsInput_Basic>{
    param: <SalaryAdditionsAndDeductionsInput_Param>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: [],
    selectedData: <SalaryAdditionsAndDeductionsInputDto>{}
  }
  paramSource = signal<SalaryAdditionsAndDeductionsInput_Basic>(structuredClone(this.initData));
  source$ = toObservable(this.paramSource);
  setSource = (data: SalaryAdditionsAndDeductionsInput_Basic) => this.paramSource.set(data)
  clearParams = () => {
    this.paramSource.set(structuredClone(this.initData))
  }
  constructor(private http: HttpClient) { }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }
  getListAddDedType() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListAddDedType', { params: { language: this.language } });
  }
  getListAddDedItem() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListAddDedItem', { params: { language: this.language } });
  }
  getListCurrency() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'getListCurrency', { params: { language: this.language } });
  }
  getListDepartment(factory: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params: { language: this.language, factory } });
  }

  getData(pagination: PaginationParam, param: SalaryAdditionsAndDeductionsInput_Param) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<SalaryAdditionsAndDeductionsInputDto>>(this.apiUrl + 'GetData', { params })
  }
  create(data: SalaryAdditionsAndDeductionsInputDto) {
    return this.http.post<OperationResult>(this.apiUrl + "Create", data);
  }

  update(data: SalaryAdditionsAndDeductionsInputDto) {
    return this.http.put<OperationResult>(this.apiUrl + "Update", data);
  }

  delete(data: SalaryAdditionsAndDeductionsInputDto) {
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { body: data });
  }

  download(param: SalaryAdditionsAndDeductionsInput_Param) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadFileExcel', { params })
  }
  downloadTemplate() {
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadFileTemplate')
  }

  upload(file: FormData) {
    file.append('language', this.language);
    return this.http.post<OperationResult>(this.apiUrl + 'UploadFileExcel', file, {});
  }

}
