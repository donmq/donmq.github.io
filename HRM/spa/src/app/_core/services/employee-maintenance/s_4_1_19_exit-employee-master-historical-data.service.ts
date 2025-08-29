import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { environment } from '@env/environment';
import {
  ExitEmployeeMasterFileHistoricalDataDto,
  ExitEmployeeMasterFileHistoricalDataMainMemory,
  ExitEmployeeMasterFileHistoricalDataParam,
  ExitEmployeeMasterFileHistoricalDataSource,
  ExitEmployeeMasterFileHistoricalDataView,
} from '@models/employee-maintenance/4_1_19_exit-employee-master-file-historical-data';
import { KeyValuePair } from '@utilities/key-value-pair';
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
export class S_4_1_19_ExitEmployeeMasterFileHistoricalDataService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl: string = environment.apiUrl + 'C_4_1_19_ExitEmployeeMasterFileHistoricalData/';
  initData: ExitEmployeeMasterFileHistoricalDataMainMemory = <ExitEmployeeMasterFileHistoricalDataMainMemory>{
    param: <ExitEmployeeMasterFileHistoricalDataParam>{},
    pagination: <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0,
    },
    data: [],
  };
  //#region Set data với Signal
  paramSearch = signal<ExitEmployeeMasterFileHistoricalDataMainMemory>(JSON.parse(JSON.stringify(this.initData)));
  paramSearch$ = toObservable(this.paramSearch);
  setParamSearch = (data: ExitEmployeeMasterFileHistoricalDataMainMemory) => this.paramSearch.set(data);

  paramForm = new BehaviorSubject<ExitEmployeeMasterFileHistoricalDataSource>(null);
  paramForm$ = this.paramForm.asObservable();
  setParamForm = (item: ExitEmployeeMasterFileHistoricalDataSource) => this.paramForm.next(item);
  //#endregion

  constructor(private http: HttpClient) { }
  clearParams = () => {
    this.paramSearch.set(JSON.parse(JSON.stringify(this.initData)))
    this.paramForm.next(null);
  }

  getPagination(pagination: PaginationParam, param: ExitEmployeeMasterFileHistoricalDataParam) {
    param.language = this.language
    var params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<ExitEmployeeMasterFileHistoricalDataView>>(
      `${this.baseUrl}GetPagination`, { params }
    );
  }

  getDetail(uSER_GUID: string, resign_Date: string) {
    var params = new HttpParams().appendAll({ uSER_GUID, resign_Date });
    return this.http.get<ExitEmployeeMasterFileHistoricalDataDto>(`${this.baseUrl}GetDetail`, { params });
  }

  //#region Get List
  getListNationality() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListNationality`, { params: { language: this.language } }
    );
  }

  getListDivision() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListDivision`, { params: { language: this.language } }
    );
  }

  getListFactory(division: string,) {
    let params = new HttpParams().appendAll({ division, language: this.language });
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListFactory`, { params }
    );
  }

  getListDepartment(division: string, factory: string,) {
    var params = new HttpParams().appendAll({ division, factory, language: this.language });
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListDepartment`, { params });
  }

  getListPermission() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListPermission`, { params: { language: this.language } }
    );
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

  getListProvinceDirectly(char1: string) {
    var params = new HttpParams().appendAll({ char1, language: this.language }

    );
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListProvinceDirectly`, { params });
  }

  getListCity(char1: string) {
    var params = new HttpParams().appendAll({ char1, language: this.language }

    );
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListCity`, { params });
  }

  getListPositionGrade() {
    return this.http.get<KeyValuePair[]>(`${this.baseUrl}GetPositionGrade`);
  }

  getListPositionTitle(level: number,) {
    var params = new HttpParams().appendAll({ level, language: this.language }

    );
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetPositionTitle`, { params });
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

  getListWorkTypeShift() {
    return this.http.get<KeyValuePair[]>(
      `${this.baseUrl}GetListWorkTypeShift`, { params: { language: this.language } }
    );
  }
  //#endregion
}
