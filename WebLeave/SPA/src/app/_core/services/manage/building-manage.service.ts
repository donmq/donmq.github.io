import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from '@env/environment';
import { BuildingInformation } from '../../models/manage/building-manage/building-information';
import { KeyValuePair } from '@utilities/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class BuildingManageService {
  apiUrl = environment.apiUrl;
  buildingSource = new BehaviorSubject<BuildingInformation>(null);
  constructor(private http: HttpClient) { }
  getBuildings() {
    return this.http.get<BuildingInformation[]>(this.apiUrl + 'BuildingManagement/GetAll');
  }
  getListArea() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'BuildingManagement/GetListArea');
  }
  addBuilding(model: BuildingInformation) {
    return this.http.post<boolean>(this.apiUrl + 'BuildingManagement/Add', model);
  }
  editBuilding(model: BuildingInformation) {
    return this.http.put<boolean>(this.apiUrl + 'BuildingManagement/Edit', model);
  }
}
