import { HttpClient, HttpParams } from '@angular/common/http';
import { EventEmitter, Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import {
  CheckBlackList,
  DepartmentSupervisorList,
  EmployeeBasicInformationMaintenanceDto,
  EmployeeBasicInformationMaintenanceMainMemory,
  EmployeeBasicInformationMaintenanceParam,
  EmployeeBasicInformationMaintenanceSource,
  EmployeeBasicInformationMaintenanceView,
  checkDuplicateParam,
} from '@models/employee-maintenance/4_1_1_employee-basic-information-maintenance';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import {
  Pagination,
  PaginationParam,
  PaginationResult,
} from '@utilities/pagination-utility';
import { BehaviorSubject } from 'rxjs';
import { IClearCache } from '@services/cache.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';

@Injectable({
  providedIn: 'root',
})
export class S_4_1_1_EmployeeBasicInformationMaintenanceService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl: string = environment.apiUrl + 'C_4_1_1_EmployeeBasicInformationMaintenance/';
  initData: EmployeeBasicInformationMaintenanceMainMemory = <EmployeeBasicInformationMaintenanceMainMemory>{
    param: <EmployeeBasicInformationMaintenanceParam>{
      crossFactoryStatus: '',
      employmentStatus: ''
    },
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0,
    },
    data: [],
  };
  //#region Set data với Signal
  paramSearch = signal<EmployeeBasicInformationMaintenanceMainMemory>(JSON.parse(JSON.stringify(this.initData)));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: EmployeeBasicInformationMaintenanceMainMemory) => this.paramSearch.set(data);

  paramForm = new BehaviorSubject<EmployeeBasicInformationMaintenanceSource>(null);
  paramForm$ = this.paramForm.asObservable();
  setParamForm = (item: EmployeeBasicInformationMaintenanceSource) => this.paramForm.next(item);
  //#endregion

  parentFun: EventEmitter<any> = new EventEmitter();
  tranferChange: EventEmitter<EmployeeBasicInformationMaintenanceSource> = new EventEmitter<EmployeeBasicInformationMaintenanceSource>();

  constructor(private http: HttpClient) { }
  clearParams() {
    this.paramSearch.set(JSON.parse(JSON.stringify(this.initData)))
    this.paramForm.next(null);
  }

  getPagination(pagination: PaginationParam, param: EmployeeBasicInformationMaintenanceParam) {
    param.language = this.language
    var params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<EmployeeBasicInformationMaintenanceView>>(
      `${this.baseUrl}GetPagination`, { params }
    );
  }

  getDetail(uSER_GUID: string) {
    var params = new HttpParams().appendAll({ uSER_GUID });
    return this.http.get<EmployeeBasicInformationMaintenanceDto>(
      `${this.baseUrl}GetDetail`, { params }
    );
  }

  add(dto: EmployeeBasicInformationMaintenanceDto) {
    return this.http.post<OperationResult>(`${this.baseUrl}Add`, dto);
  }

  edit(dto: EmployeeBasicInformationMaintenanceDto) {
    return this.http.put<OperationResult>(`${this.baseUrl}Edit`, dto);
  }

  rehire(dto: EmployeeBasicInformationMaintenanceDto) {
    return this.http.put<OperationResult>(`${this.baseUrl}Rehire`, dto);
  }

  delete(useR_GUID: string) {
    var params = new HttpParams().appendAll({ useR_GUID });
    return this.http.delete<OperationResult>(`${this.baseUrl}Delete`, { params });
  }

  downloadExcelTemplate() {
    return this.http.get<OperationResult>(`${this.baseUrl}DownloadExcelTemplate`);
  }

  uploadExcel(file: FormData) {
    return this.http.post<OperationResult>(`${this.baseUrl}UploadExcel`, file)
  }

  checkDuplicateCase1(nationality: string, identificationNumber: string) {
    var params = new HttpParams().appendAll({ nationality, identificationNumber });
    return this.http.get<boolean>(`${this.baseUrl}CheckDuplicateCase1`, { params });
  }

  checkDuplicateCase2(param: checkDuplicateParam) {
    var params = new HttpParams().appendAll({ ...param });
    return this.http.get<boolean>(`${this.baseUrl}CheckDuplicateCase2`, { params });
  }

  checkDuplicateCase3(param: checkDuplicateParam) {
    var params = new HttpParams().appendAll({ ...param });
    return this.http.get<boolean>(`${this.baseUrl}CheckDuplicateCase3`, { params });
  }

  checkBlackList(param: CheckBlackList) {
    var params = new HttpParams().appendAll({ ...param });
    return this.http.get<boolean>(`${this.baseUrl}CheckBlackList`, { params });
  }

  //#region Get List
  getListDivision() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListDivision`, { params: { language: this.language } }
    );
  }
  getListFactory(division: string) {
    var params = new HttpParams().appendAll({ division, language: this.language });
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListFactory`, { params }

    );
  }
  getListNationality() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListNationality`, { params: { language: this.language } }

    );
  }
  getListPermission() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListPermission`, { params: { language: this.language } });
  }
  getListIdentityType() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListIdentityType`, { params: { language: this.language } }
    );
  }
  getListEducation() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListEducation`, { params: { language: this.language } }
    );
  }
  getListWorkType() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListWorkType`, { params: { language: this.language } }
    );
  }
  getListRestaurant() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListRestaurant`, { params: { language: this.language } }
    );
  }
  getListReligion() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListReligion`, { params: { language: this.language } }
    );
  }
  getListTransportationMethod() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListTransportationMethod`, { params: { language: this.language } }
    );
  }
  getListVehicleType() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListVehicleType`, { params: { language: this.language } }
    );
  }
  getListWorkLocation() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListWorkLocation`, { params: { language: this.language } }
    );
  }
  getListReasonResignation() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListReasonResignation`, { params: { language: this.language } }
    );
  }

  getListDepartment(division: string, factory: string) {
    var params = new HttpParams().appendAll({ division, factory, language: this.language });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetListDepartment`, { params });
  }

  getListProvinceDirectly(char1: string,) {
    var params = new HttpParams().appendAll({ char1, language: this.language });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetListProvinceDirectly`, { params });
  }

  getListCity(char1: string) {
    var params = new HttpParams().appendAll({ char1, language: this.language });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetListCity`, { params });
  }
  getListPositionGrade() {
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetPositionGrade`);
  }

  getListPositionTitle(level: number) {
    var params = new HttpParams().appendAll({ level, language: this.language });
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetPositionTitle`, { params, });
  }

  getDepartmentSupervisor(uSER_GUID: string) {
    var params = new HttpParams().appendAll({ uSER_GUID, language: this.language });
    return this.http.get<DepartmentSupervisorList[]>(`${this.baseUrl}GetDepartmentSupervisor`, { params });
  }

  getListWorkTypeShift() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListWorkTypeShift`, { params: { language: this.language } }
    );
  }
  //#endregion
}
