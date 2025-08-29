import { Injectable, signal } from '@angular/core';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { environment } from '@env/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { UserForLogged } from '@models/auth/auth';
import {
  Leave_Record_Modification_MaintenanceDto,
  HRMS_Att_Leave_MaintainSearchParamDto,
  LeaveRecordModificationMaintenanceSource
} from '@models/attendance-maintenance/5_1_19_leave-record-modification-maintenance';
import { KeyValuePair } from '@utilities/key-value-pair';
import { IClearCache } from '@services/cache.service';
@Injectable({
  providedIn: 'root'
})
export class S_5_1_19_LeaveRecordModificationMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  user: UserForLogged = JSON.parse((localStorage.getItem(LocalStorageConstants.USER)));
  constructor(private http: HttpClient) { }
  apiUrl = `${environment.apiUrl}C_5_1_19_LeaveRecordModificationMaintenance/`;
  apiCommonUrl = `${environment.apiUrl}Common/`;
  initData = <LeaveRecordModificationMaintenanceSource>(<LeaveRecordModificationMaintenanceSource>{
    param: <HRMS_Att_Leave_MaintainSearchParamDto>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0
    },
    selectedData: <Leave_Record_Modification_MaintenanceDto>{},
    data: []
  });

  paramSearch = signal<LeaveRecordModificationMaintenanceSource>(structuredClone(this.initData))
  setSource = (data: LeaveRecordModificationMaintenanceSource) => this.paramSearch.set(data)
  clearParams = () => {
    this.paramSearch.set(structuredClone(this.initData))
  }
  getData(pagination: PaginationParam, param: HRMS_Att_Leave_MaintainSearchParamDto) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<Leave_Record_Modification_MaintenanceDto>>(this.apiUrl + "GetData", { params })
  }

  addNew(param: Leave_Record_Modification_MaintenanceDto) {
    return this.http.post<OperationResult>(`${this.apiUrl}Create`, param);
  }

  edit(param: Leave_Record_Modification_MaintenanceDto) {
    return this.http.put<OperationResult>(this.apiUrl + "Update", param);
  }

  delete(param: Leave_Record_Modification_MaintenanceDto) {
    return this.http.post<OperationResult>(this.apiUrl + "Delete", param);
  }

  getListHoliday(typeSeq: string, kind: number, inputChar: string) {
    return this.http.get<KeyValuePair[]>(this.apiCommonUrl + 'GetListBasicCodeListChar1', { params: { language: this.language, typeSeq, kind, inputChar } });
  }

  getListPermissionGroup() {
    return this.http.get<KeyValuePair[]>(this.apiCommonUrl + 'GetListPermissionGroup', { params: { language: this.language } });
  }

  checkExistedData(data: Leave_Record_Modification_MaintenanceDto) {
    let params = new HttpParams()
      .set('Factory', data.factory)
      .set('Employee_ID', data.employee_ID)
      .set('Leave_Code', data.leave_Code)
      .set('Leave_Date', data.leave_Date_Str)
    return this.http.get<OperationResult>(`${this.apiUrl}CheckExistedData`, { params });
  }
  GetListLeave() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListLeave', { params: { language: this.language } });
  }
  getListFactoryByUser() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactoryByUser', { params: { language: this.language } });
  }
  getWorkShiftType(data: Leave_Record_Modification_MaintenanceDto) {
    let param = <HRMS_Att_Leave_MaintainSearchParamDto>{
      factory: data.factory,
      employee_ID: data.employee_ID,
      leave_Date_Str: data.leave_Date_Str,
      lang: this.language
    }
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + "GetWorkShiftType", { params });
  }

  download(param: HRMS_Att_Leave_MaintainSearchParamDto) {
    param.lang = this.language
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResult>(this.apiUrl + 'DownloadFileExcel', { params })
  }
}
