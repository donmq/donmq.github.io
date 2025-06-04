import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class UserRoleManageService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllRoleUser(userId: number, langId: string) {
    let params = new HttpParams().appendAll({ userId, langId });
    return this.http.get<any>(this.apiUrl + 'UserRoles/GetAllRoleUser', { params });
  }

  getAllGroupBase(userId: number, langId: string) {
    let params = new HttpParams().appendAll({ userId, langId });
    return this.http.get<any>(this.apiUrl + 'UserRoles/GetAllGroupBase', { params });
  }

  assignRole(userId: number, roleId: number): Observable<OperationResult> {
    let params = new HttpParams().appendAll({ roleId });
    return this.http.post<OperationResult>(this.apiUrl + 'UserRoles/AssignRole/' + userId, null, { params });
  }

  unAssignRole(userId: number, roleId: number): Observable<OperationResult> {
    let params = new HttpParams().appendAll({ roleId });
    return this.http.post<OperationResult>(this.apiUrl + 'UserRoles/UnAssignRole/' + userId, null, { params });
  }

  updateRoleRank(userId: number, roleRank: number, isInherit: boolean): Observable<OperationResult> {
    let params = new HttpParams().appendAll({ userId, roleRank, isInherit });
    return this.http.post<OperationResult>(this.apiUrl + 'UserRoles/UpdateRoleRank', null, { params });
  }

  assignGroupBase(userId: number, gbId: number): Observable<OperationResult> {
    let params = new HttpParams().appendAll({ gbId, userId });
    return this.http.post<OperationResult>(this.apiUrl + 'UserRoles/AssignGroupBase', null, { params });
  }

  unAssignGroupBase(userId: number, gbId: number): Observable<OperationResult> {
    let params = new HttpParams().appendAll({ gbId, userId });
    return this.http.post<OperationResult>(this.apiUrl + 'UserRoles/UnAssignGroupBase', null, { params });
  }

  setPermit(userId: number, key: string): Observable<OperationResult> {
    let params = new HttpParams().appendAll({ userId, key });
    return this.http.post<OperationResult>(this.apiUrl + 'UserRoles/SetPermit', null, { params });
  }

  removePermit(userId: number, key: string): Observable<OperationResult> {
    let params = new HttpParams().appendAll({ userId, key });
    return this.http.post<OperationResult>(this.apiUrl + 'UserRoles/RemovePermit', null, { params });
  }

  setReport(userId: number, key: string): Observable<OperationResult> {
    let params = new HttpParams().appendAll({ userId, key });
    return this.http.post<OperationResult>(this.apiUrl + 'UserRoles/SetReport', null, { params });
  }

  removeReport(userId: number, key: string): Observable<OperationResult> {
    let params = new HttpParams().appendAll({ userId, key });
    return this.http.post<OperationResult>(this.apiUrl + 'UserRoles/RemoveReport', null, { params });
  }

  exportExcel(userId: number, langId: string) {
    let params = new HttpParams().appendAll({ userId, langId });
    return this.http.get<OperationResult>(this.apiUrl + 'UserRoles/ExportExcel', { params })
  }

  listUsers(roleID: number) {
    let params = new HttpParams().appendAll({ roleID });
    return this.http.get<string[]>(this.apiUrl + 'UserRoles/ListUsers', { params })
  }
}
