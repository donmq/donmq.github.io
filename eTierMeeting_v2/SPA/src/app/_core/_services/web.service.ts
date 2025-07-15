import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class WebService {

  private _jsonURL = 'assets/i18n/';

  listNavAdmin = new BehaviorSubject<any>(null);


  constructor(private http: HttpClient) { }

  setLanguage(culture: string) {
    return this.http.get(`${API}User/SetLanguage?culture=${culture}`, { withCredentials: true });
  }

}
