import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { KeyValuePair } from '../_utilities/key-value-pair';
import { Pagination, PaginationResult } from '../_utilities/pagination-helper';
import { T2MeetingTimeSettingParam } from '../_helpers/params/t2-meeting-time-setting.param';
import { eTM_T2_Meeting_Seeting } from '../_models/eTM_T2_Meeting_Seeting';
import { OperationResult } from '../_utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class T2MeetingTimeSettingService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getListBuildingOrGroup() { 
    return this.http.get<KeyValuePair[]>(this.baseUrl + "T2MeetingTimeSetting/GetListBuildingOrGroup");
  }

  getAllData(pagination: Pagination, param: T2MeetingTimeSettingParam) { 
    let params = new HttpParams().appendAll({ "pageNumber": pagination.pageNumber+"", "pageSize": pagination.pageSize+"", ...param });
    return this.http.get<PaginationResult<eTM_T2_Meeting_Seeting>>(this.baseUrl + "T2MeetingTimeSetting/GetAllData", { params });
  }

  delete(item: eTM_T2_Meeting_Seeting) { 
    let params = new HttpParams().appendAll({...item});
    return this.http.delete<OperationResult>(this.baseUrl + "T2MeetingTimeSetting/Delete", { params });
  }

  add(newItem: eTM_T2_Meeting_Seeting) { 
    return this.http.post<OperationResult>(this.baseUrl + "T2MeetingTimeSetting/Add", newItem);
  }

}
