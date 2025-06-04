import { HttpParams } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { HolidayAndUserDto } from '@models/manage/holiday/holiday-and-user';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class HolidayService {
  apiUrl = environment.apiUrl;

  constructor(
    private http: HttpClient,
  ) { }

  getHolidayData() {
    return this.http.get<HolidayAndUserDto[]>(this.apiUrl + 'Holiday/GetHoliday');
  }

  removeHoliday(IDHoliday: number) {
    let params = new HttpParams()
      .set('IDHoliday', IDHoliday)
    return this.http.delete<HolidayAndUserDto>(this.apiUrl + 'Holiday/RemoveHoliday', {params});
  }

  addHoliday(model: HolidayAndUserDto) {
    return this.http.post<OperationResult>(this.apiUrl + 'Holiday/AddHoliday', model)
  }

}
