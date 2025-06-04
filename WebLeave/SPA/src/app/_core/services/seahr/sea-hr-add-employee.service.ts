import { ListPosition } from '@models/seahr/listPosition';
import { ListGroupBase } from '@models/seahr/listGroupBase';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { Employee } from '@models/seahr/employee';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class SeaHrAddEmployeeService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private spinnerService: NgxSpinnerService,) { }

  getAllDepartment() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'SeaHrAddEmployee/GetListDepartment', {});
  }

  getAllPosition() {
    return this.http.get<ListPosition[]>(this.baseUrl + 'SeaHrAddEmployee/GetListPosition', {});
  }

  getAllPart(departmentId: number, lang: string) {
    let params = new HttpParams()
      .set('departmentId', departmentId)
      .set('lang', lang);
    return this.http.get<KeyValuePair[]>(this.baseUrl + 'SeaHrAddEmployee/GetListPart', { params });
  }

  getAllGroupBase() {
    return this.http.get<ListGroupBase[]>(this.baseUrl + 'SeaHrAddEmployee/GetListGroupBase', {});
  }

  addEmployee(employee: Employee) {
    return this.http.post<OperationResult>(this.baseUrl + 'SeaHrAddEmployee/AddEmployee', employee);
  }

  uploadExcel(file: File): Observable<OperationResult> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<OperationResult>(this.baseUrl + 'SeaHrAddEmployee/UploadExcel', formData)
  }

  exportExcel() {
    this.spinnerService.show();
    return this.http.get(this.baseUrl + 'SeaHrAddEmployee/ExportExcel', { responseType: 'blob' }).subscribe((result: Blob) => {
      this.spinnerService.hide();
      const blob = new Blob([result]);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      const fileName = 'Sample_AddListEmployee' + '.xlsx';
      link.href = url;
      link.setAttribute('download', fileName);
      document.body.appendChild(link);
      link.click();
    });
  }
}
