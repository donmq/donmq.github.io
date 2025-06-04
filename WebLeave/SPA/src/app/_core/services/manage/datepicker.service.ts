import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { Datepicker } from '@models/manage/datepicker-management/datepicker';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class DatepickerService {
  baseUrl = environment.apiUrl
  constructor(private http: HttpClient) { }

  getAllDatepicker() {
    return this.http.get<Datepicker[]>(`${this.baseUrl}Datepicker/GetAll`);
  }

  updateDatepicker(datepicker: Datepicker) {
    return this.http.put<OperationResult>(`${this.baseUrl}Datepicker/UpdateDatepicker`, datepicker);
  }
}
