import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ResultResponse, UserForLogged, UserLoginParam } from '@models/auth/auth';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { CacheService } from './../cache.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiUrl = environment.apiUrl;
  jwtHelper = new JwtHelperService();

  constructor(
    private http: HttpClient,
    private router: Router,
    private cache: CacheService,
  ) { }

  login(param: UserLoginParam) {
    return this.http.post(this.apiUrl + 'Auth/login', param).pipe(
      map((response: ResultResponse) => {
        if (response) {
          localStorage.setItem(LocalStorageConstants.TOKEN, response.token);
          localStorage.setItem(LocalStorageConstants.USER, JSON.stringify(response.data.user));
          localStorage.setItem(LocalStorageConstants.CODE_LANG, JSON.stringify(response.data.code_Information));
        }
      })
    );
  }

  logout = () => {
    const recentUser = localStorage.getItem(LocalStorageConstants.USER)
    if (recentUser != null) {
      this.cache.clearCache();
      localStorage.removeItem(LocalStorageConstants.USER);
      sessionStorage.clear();
    }
    this.router.navigate(['/login']);
  }

  loggedIn() {
    const token: string = localStorage.getItem(LocalStorageConstants.TOKEN);
    const user: UserForLogged = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
    const flag = !user || this.jwtHelper.isTokenExpired(token)
    return !flag
  }

  getListFactory() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'Auth/GetListFactory');
  }
  getDirection() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'Auth/GetDirection');
  }
  getProgram(direction: string) {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'Auth/GetProgram', { params: { direction } });
  }
  getListLangs() {
    return this.http.get<string[]>(this.apiUrl + 'Auth/GetListLangs');
  }
}
