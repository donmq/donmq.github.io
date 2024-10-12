import { Injectable } from '@angular/core';
import { environment } from './../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { KeyValuePair } from './../utilities/key-value-pair';

@Injectable({
  providedIn: 'root'
})
export class HomeMainService {
  baseUrl: string = environment.apiUrl + "HomeMain/"
  constructor(private http: HttpClient) { }
  getListPlayers() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListPlayers")
  }
  getListExercise() {
    return this.http.get<KeyValuePair[]>(this.baseUrl + "GetListExercise")
  }

}
