import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import { CompulsoryInsuranceDataMaintenance_Basic, CompulsoryInsuranceDataMaintenance_Personal, CompulsoryInsuranceDataMaintenanceDto, CompulsoryInsuranceDataMaintenanceParam } from '@models/compulsory-insurance-management/6_1_1_compulsory_insurance_data_maintenance';
import { IClearCache } from '@services/cache.service';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_6_1_1_Compulsory_Insurance_Data_MaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_6_1_1_CompulsoryInsuranceDataMaintenance/`;
  //signal
  initData: CompulsoryInsuranceDataMaintenance_Basic = <CompulsoryInsuranceDataMaintenance_Basic>{
    param: <CompulsoryInsuranceDataMaintenanceParam>{
      searchMethod: "InsuranceDate",
    },
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: null
  }
  paramSource = signal<CompulsoryInsuranceDataMaintenance_Basic>(structuredClone(this.initData));
  source$ = toObservable(this.paramSource);
  setSource = (data: CompulsoryInsuranceDataMaintenance_Basic) => this.paramSource.set(data)
  clearParams = () => {
    this.paramSource.set(structuredClone(this.initData))
  }

  constructor(private http: HttpClient) { }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { language: this.language } });
  }
  getListInsuranceType() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListInsuranceType', { params: { language: this.language } });
  }

  getData(pagination: PaginationParam, param: CompulsoryInsuranceDataMaintenanceParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });

    return this.http.get<PaginationResult<CompulsoryInsuranceDataMaintenanceDto>>(this.apiUrl + 'GetData', { params })
  }
  create(data: CompulsoryInsuranceDataMaintenanceDto) {
    return this.http.post<OperationResult>(this.apiUrl + "Create", data);
  }

  update(data: CompulsoryInsuranceDataMaintenanceDto) {
    return this.http.put<OperationResult>(this.apiUrl + "Update", data);
  }

  delete(data: CompulsoryInsuranceDataMaintenanceDto) {
    return this.http.delete<OperationResult>(this.apiUrl + "Delete", { body: data });
  }

  download(param: CompulsoryInsuranceDataMaintenanceParam) {
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
