import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { OperationResult } from '../_models/operation-result';
import { Pagination, PaginationResult } from '../_models/pagination';
import { User } from '../_models/user';

const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class UserService {
  messageSource = new BehaviorSubject<string>('default message');
  currentMessage = this.messageSource.asObservable();
  constructor(private http: HttpClient,
    private _spinnerService: NgxSpinnerService) { }

  changeMessage(message) {
    this.messageSource.next(message);
  }

  getAllUserPreliminary() {
    return this.http.get<User[]>(`${API}User/GetAllUserPreliminary`);
  }

  getAllUser() {
    return this.http.get<User[]>(`${API}User/GetAllUser`);
  }

  getUserName(userNumber: string) {
    return this.http.get<User>(`${API}User/GetUserName?userName=${userNumber}`);
  }

  removeUser(empNumber: string, lang: string) {
    return this.http.post<OperationResult>(`${API}User/RemoveUser?empNumber=${empNumber}&lang=${lang}`, {});
  }

  addUser(user: User, lang: string) {
    return this.http.post<OperationResult>(`${API}User/AddUser?lang=${lang}`, user);
  }

  restoreUser(empNumber: string, lang: string) {
    return this.http.put<OperationResult>(`${API}User/RestoreUser?empNumber=${empNumber}&lang=${lang}`, {});
  }
  updateUser(user: User, lang: string) {
    return this.http.post<OperationResult>(`${API}User/UpdateUser?lang=${lang}`, user);
  }

  searchUser(keyword: string, pagination: Pagination) {
    let params = new HttpParams();
    if (pagination.currentPage !== null && pagination.pageSize !== null) {
      params = params.append('pageNumber', pagination.currentPage.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }
    return this.http.post<PaginationResult<User>>(`${API}User/SearchUser?keyword=${keyword}`, {}, { params });
  }

  exportExcel() {
    this._spinnerService.show();
    return this.http.post(`${API}User/ExportExcel`, {}, { responseType: 'blob', withCredentials: true }).subscribe((result: Blob) => {
      if (result.type !== 'application/xlsx') {
        alert(result.type);
      }
      const blob = new Blob([result]);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      const currentTime = new Date();
      const filename =
        'ExportUser-' +
        currentTime.getFullYear().toString() +
        (currentTime.getMonth() + 1) + '_' +
        currentTime.getDate() +
        '.xlsx';
      link.href = url;
      link.setAttribute('download', filename);
      document.body.appendChild(link);
      link.click();
      this._spinnerService.hide();
    });
  }
}
