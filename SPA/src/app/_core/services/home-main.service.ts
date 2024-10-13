import { Injectable } from '@angular/core';
import { environment } from './../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { KeyValuePair } from './../utilities/key-value-pair';
import { MainHomeDto, MainHomeParam } from '../models/home';

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

}
