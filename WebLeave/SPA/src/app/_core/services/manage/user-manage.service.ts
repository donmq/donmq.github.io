import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '@env/environment';
import { Users } from '../../models/manage/user-manage/Users';
import { OperationResult } from '@utilities/operation-result';
import { PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { UserManageParam, UserManageTitleExcel } from '@params/manage/user-manage-param';

@Injectable({
  providedIn: 'root'
})
export class UserManageService {
  apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  dataSource = new BehaviorSubject<UserManageParam>(null)
  currentDataSource = this.dataSource.asObservable();

  getAll(pagination: PaginationParam, param: UserManageParam): Observable<PaginationResult<Users>> {
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<Users>>(this.apiUrl + 'User/GetAll', { params });
  }
  getUser(userId: number): Observable<Users> {
    let params = new HttpParams().appendAll({ userId });
    return this.http.get<Users>(this.apiUrl + 'User', { params });
  }

  addUser(user: Users): Observable<OperationResult> {
    return this.http.post<OperationResult>(this.apiUrl + 'User/Add', user);
  }

  editUser(user: Users): Observable<OperationResult> {
    return this.http.put<OperationResult>(this.apiUrl + 'User/Edit', user);
  }

  uploadExcel(file: File): Observable<OperationResult> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<OperationResult>(this.apiUrl + 'User/UploadExcel', formData);
  }

  exportExcel(titleExcel: UserManageTitleExcel, param: UserManageParam, lang: string) {
    let params = new HttpParams().appendAll({ ...titleExcel, ...param, lang });
    return this.http.get<OperationResult>(this.apiUrl + 'User/ExportExcel', {params})
  }
}
