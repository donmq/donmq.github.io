import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../environments/environment';
import { Employee } from '../_models/employee';
import { EmployAdmin } from '../_models/employee-admin';
import { OperationResult } from '../_models/operation-result';
import { Pagination, PaginationResult } from '../_models/pagination';

const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http: HttpClient,
    private _spinnerService: NgxSpinnerService) { }

  getEmployee(empNumber: any) {
    let params = new HttpParams();
    if (empNumber !== null) {
      params = params.append('empNumber', empNumber);
    }
    return this.http.post<any>(`${API}Employee/GetEmployeeScanByNumber`, empNumber, { params });
  }

  removeEmployee(empNumber: string, lang: string) {
    return this.http.post<OperationResult>(`${API}Employee/RemoveEmploy?empNumber=${empNumber}&lang=${lang}`, {});
  }

  addEmployee(employee: Employee, lang: string) {
    return this.http.post<OperationResult>(`${API}Employee/AddEmploy?lang=${lang}`, employee);
  }

  updateEmployee(employee: Employee, lang: string) {
    return this.http.post<OperationResult>(`${API}Employee/UpdateEmploy?lang=${lang}`, employee);
  }

  searchEmployee(keyword: string, pagination: Pagination) {
    let params = new HttpParams();
    if (pagination.currentPage !== null && pagination.pageSize !== null) {
      params = params.append('pageNumber', pagination.currentPage.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }
    return this.http.post<PaginationResult<EmployAdmin>>(`${API}Employee/SearchEmploy?keyword=${keyword}`, {}, { params });
  }

  exportExcel() {
    this._spinnerService.show();
    return this.http.post(`${API}Employee/ExportExcel`, {}, { responseType: 'blob', withCredentials: true }).subscribe((result: Blob) => {
      if (result.type !== 'application/xlsx') {
        alert(result.type);
      }
      const blob = new Blob([result]);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      const currentTime = new Date();
      const filename =
        'ExportEmployee-' +
        currentTime.getFullYear().toString() +
        (currentTime.getMonth() + 1) + "_" +
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
