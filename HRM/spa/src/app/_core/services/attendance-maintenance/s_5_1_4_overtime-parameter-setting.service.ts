import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '@env/environment';
import {
  HRMS_Att_Overtime_ParameterDTO,
  HRMS_Att_Overtime_ParameterParam,
  HRMS_Att_Overtime_Parameter_Basic
} from '@models/attendance-maintenance/5_1_4_overtime-parameter-setting';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { toObservable } from '@angular/core/rxjs-interop';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root'
})
export class S_5_1_4_OvertimeParameterSettingService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  apiUrl = `${environment.apiUrl}C_5_1_4_OvertimeParameterSetting/`;
  //signal
  initData: HRMS_Att_Overtime_Parameter_Basic = <HRMS_Att_Overtime_Parameter_Basic>{
    param: <HRMS_Att_Overtime_ParameterParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    data: null,
  }
  paramSource = signal<HRMS_Att_Overtime_Parameter_Basic>(structuredClone(this.initData));
  source$ = toObservable(this.paramSource);
  setSource = (data: HRMS_Att_Overtime_Parameter_Basic) => this.paramSource.set(data)
  clearParams = () => {
    this.paramSource.set(structuredClone(this.initData))
  }
  constructor(private http: HttpClient) { }
  getListDivision() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListDivision', { params: { language: this.language } });
  }
  getListFactory(division: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params: { division, language: this.language } });
  }
  getListWorkShiftType() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListWorkShiftType', { params: { language: this.language } });
  }

  getData(pagination: PaginationParam, param: HRMS_Att_Overtime_ParameterParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<HRMS_Att_Overtime_ParameterDTO>>(this.apiUrl + 'GetData', { params })
  }

  create(data: HRMS_Att_Overtime_ParameterDTO) {
    return this.http.post<OperationResult>(this.apiUrl + "Create", data);
  }

  update(data: HRMS_Att_Overtime_ParameterDTO) {
    return this.http.put<OperationResult>(this.apiUrl + "Update", data);
  }

  download(param: HRMS_Att_Overtime_ParameterParam) {
    param.language = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadFileExcel', { params })
  }
  downloadTemplate() {
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadFileTemplate')
  }

  upload(file: FormData) {
    return this.http.post<OperationResult>(this.apiUrl + 'UploadFileExcel', file, {});
  }
}
