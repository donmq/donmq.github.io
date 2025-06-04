import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddManuallyParam } from '@params/seahr/add-manually-param';
import { KeyValuePair } from '@utilities/key-value-pair';
import { environment } from '@env/environment';
import { AddManuallyViewModel } from '@models/seahr/seahr/add-manually-view-model';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})

export class SeaHrAddManuallyService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getDetail(leaveId: number) {
    let params = new HttpParams().set('leaveId', leaveId);
    return this.http.get<AddManuallyViewModel>(this.baseUrl + "AddManually/GetDetail", { params })
  }

  addManually(params: AddManuallyParam) {
    return this.http.post<OperationResult>(this.baseUrl + "AddManually/Create", params)
  }

  delete(leaveId: string) {
    let params = new HttpParams().set('leaveId', leaveId);
    return this.http.delete<OperationResult>(this.baseUrl + "AddManually/DeleteManually", { params })
  }
  getAllCategory(languageId: string) {
    let params = new HttpParams().set('languageId', languageId);
    return this.http.get<KeyValuePair[]>(this.baseUrl + "AddManually/category", { params })
  }

  getCountRestAgent(year: number, empNumber: string) {
    let params = new HttpParams().set('year', year).set('empNumber', empNumber);
    return this.http.get<number>(`${this.baseUrl}AddManually/GetCountRestAgent`, { params });
  }

  checkDateLeave(start: string, end: string, empNumber: string) {
    let params = new HttpParams().set('start', start).set('end', end).set('empNumber', empNumber);
    return this.http.get<any>(`${this.baseUrl}AddManually/CheckDateLeave`, { params });
  }
  checkIsSun(empNumber: string) {
    let params = new HttpParams().set('empNumber', empNumber);
    return this.http.get<boolean>(`${this.baseUrl}AddManually/CheckIsSun`, { params });
  }
}
