import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserForLoginDto } from '../_models/user';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { OperationResult } from '../_models/operation-result';
import { ChangePassword } from '../_models/change-password';
import { DEFAULT_INTERRUPTSOURCES, Idle } from '@ng-idle/core';
import { ServerInfo } from '../_models/server-info';


const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  userLogin = new BehaviorSubject<any>({});
  urlBeforeRedit = new BehaviorSubject<string>('');

  private callLogout: () => void;

  constructor(
    private http: HttpClient,
    private _router: Router,
    private idle: Idle
  ) { }

  login(users: UserForLoginDto) {
    return this.http.post(`${API}Auth/login`, users).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('user', JSON.stringify(user.user));
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.userLogin.next(this.decodedToken);
          this.urlBeforeRedit.pipe(map((res: string) => res)).subscribe(val => {
            if (val !== '') {
              this._router.navigateByUrl(val);
            }
          }).unsubscribe();
        }
      }),
    );
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    const check = !this.jwtHelper.isTokenExpired(token);
    if (check) {
      this.userLogin.next(this.jwtHelper.decodeToken(token));
    }
    return check;
  }

  changePassword(changePassword: ChangePassword, lang: string) {
    return this.http.post<OperationResult>(`${API}Auth/ChangePassword?lang=${lang}`, changePassword);
  }

  setTime() {
    this.idle.setIdle(3600);
    this.idle.setTimeout(60);
    this.idle.setInterrupts(DEFAULT_INTERRUPTSOURCES);
    this.idle.onTimeout.subscribe(() => {
      this.callLogout();
    });
    this.idle.watch();
  }

  onLoggedOut(fn: () => void) {
    this.callLogout = fn;
  }

  getServerInfo() {
    return this.http.get<ServerInfo>(`${API}Auth/ServerInfo`);
  }
}
