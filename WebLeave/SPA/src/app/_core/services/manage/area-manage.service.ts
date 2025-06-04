import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from '@env/environment';
import { AreaInformation } from '../../models/manage/area-manage/area-infomation';

@Injectable({
  providedIn: 'root'
})
export class AreaManageService {
  apiUrl = environment.apiUrl;
  areaSource = new BehaviorSubject<AreaInformation>(null);
  constructor(private http: HttpClient) { }
  getAreas() {
    return this.http.get<AreaInformation[]>(this.apiUrl + 'AreaManagement/GetAll');
  }

  addArea(param: AreaInformation) {
    return this.http.post<boolean>(this.apiUrl + 'AreaManagement/Add',param);
  }

  editArea(param: AreaInformation) {
    return this.http.put<boolean>(this.apiUrl + 'AreaManagement/Edit', param);
  }
}
