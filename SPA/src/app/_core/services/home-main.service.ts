import { Injectable } from '@angular/core';
import { environment } from './../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { KeyValuePair } from './../utilities/key-value-pair';
import { DataCreate, MainHomeDto, MainHomeParam, Quality } from '../models/home';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class HomeMainService {
  baseUrl: string = environment.apiUrl + "HomeMain/"
  constructor(private http: HttpClient) { }

  getData(param: MainHomeParam) {
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<MainHomeDto>(this.baseUrl + "GetData", { params })
  }

  getListPlayers() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListPlayers")
  }

  getListExercise() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListExercise")
  }
  getKeys() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetKeys")
  }

  getListThuocTinh(exerciseID: number, position: string) {
    let params = new HttpParams().appendAll({ exerciseID, position });
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListThuocTinh", { params })
  }

  getExercisesForAttributes(key: string) {
    let params = new HttpParams().appendAll({ key });
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetExercisesForAttributes", { params })
  }

  getListDisable(position: string) {
    let params = new HttpParams().appendAll({ position });
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListDisable", { params })
  }
  create(data: DataCreate) {
    return this.http.post<OperationResult>(this.baseUrl + 'Create', data)
  }
  update(data: DataCreate) {
    return this.http.put<OperationResult>(this.baseUrl + 'Update', data)
  }
  delete(id: number) {
    let params = new HttpParams().appendAll({ id })
    return this.http.delete<OperationResult>(this.baseUrl + 'Delete', { params });
  }

  getListCompares(inforID: number) {
    let params = new HttpParams().appendAll({ inforID });
    return this.http.get<Quality[]>(this.baseUrl + "GetListCompares", { params })
  }
  createCompare(data: DataCreate) {
    return this.http.post<OperationResult>(this.baseUrl + 'CreateCompare', data)
  }
  deleteCompare(data: Quality) {
  console.log('data :', data);
    return this.http.delete<OperationResult>(this.baseUrl + 'DeleteCompare', { body: data });
  }
}
