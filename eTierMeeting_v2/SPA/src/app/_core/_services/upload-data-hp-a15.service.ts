import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { OperationResult } from '../_models/operation-result';

@Injectable({
  providedIn: 'root'
})
export class UploadDataHpA15Service {
  apiUrl: string = `${environment.apiUrl}UploadDataHPA15`;
  constructor(private http: HttpClient) { }

  uploadExcel(file: File) {
    let formData = new FormData();
    formData.append('file', file);
    return this.http.post<OperationResult>(`${this.apiUrl}/ImportDataExcel`, formData);
  }
}
