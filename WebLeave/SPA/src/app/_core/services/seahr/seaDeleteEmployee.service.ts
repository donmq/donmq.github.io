import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { OperationResult } from '@utilities/operation-result';
import { NgxSpinnerService } from 'ngx-spinner';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SeaDeleteEmployeeService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private spinnerService: NgxSpinnerService) {}

  uploadExcel(file: File) {
    var formData = new FormData();
    formData.append('file', file);
    return this.http.post<OperationResult>(
      this.baseUrl + 'HrDeleteEmployee/UploadExcelDelete',
      formData
    );
  }
  downloadExcel() {
    return this.http.get(this.baseUrl + 'HrDeleteEmployee/DowloadFile', { responseType: 'blob' })
      .pipe(
        tap((result: Blob) => {
          const blob = new Blob([result]);
          const url = window.URL.createObjectURL(blob);
          const link = document.createElement('a');
          const currentTime = new Date();
          const filename = 'ListEmployeeDelete' + currentTime.getFullYear().toString() +
            (currentTime.getMonth() + 1) + currentTime.getDate() +
            currentTime.toLocaleTimeString().replace(/[ ]|[,]|[:]/g, '').trim() + '.xlsx';
          link.href = url;
          link.setAttribute('download', filename);
          document.body.appendChild(link);
          link.click();
        })
      );
  }
}
