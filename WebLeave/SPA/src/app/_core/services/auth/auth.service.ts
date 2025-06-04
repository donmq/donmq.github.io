import { HttpClient, HttpParams } from "@angular/common/http";
import { EventEmitter, Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { BehaviorSubject, firstValueFrom, Observable, of } from "rxjs";
import { tap } from "rxjs/operators";
import { environment } from "@env/environment";
import { LocalStorageConstants } from "@constants/local-storage.enum";
import { LoginResponse } from "@models/auth/user-for-logged.model";
import { Users } from "@models/auth/users.model";
import { UserForLoginParam } from "@params/auth/user-for-login.param";
import { OperationResult } from "@utilities/operation-result";
import { ActivatedRoute, Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { NgSnotifyService } from "@services/ng-snotify.service";
import { RoleConstants } from "@constants/role.constants";
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl = environment.apiUrl;
  jwtHelper = new JwtHelperService();
  currentUser: Users;
  decodedToken: any;
  disLoginEmitter: EventEmitter<boolean> = new EventEmitter();

  constructor(
    private http: HttpClient,
    private snotifyService: NgSnotifyService,
    private translateService: TranslateService,
    private router: Router,
    private route: ActivatedRoute) { }

  login(model: UserForLoginParam): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.baseUrl + 'Auth/Login', model).pipe(
      tap(res => {
        if (res) {
          if (res.alreadyLoggedIn) {
            this.disLoginEmitter.emit(true);
            this.reLogin(model);
          } else {
            this.setLocalStore(res);
            this.router.navigate(['/dashboard']);
            this.snotifyService.success(
              this.translateService.instant('System.Message.LogIn'),
              this.translateService.instant('System.Caption.Success'));
          }
        } else {
          this.snotifyService.error(
            this.translateService.instant('System.Message.LogInFailed'),
            this.translateService.instant('System.Caption.Error'));
        }
      })
    );
  }

  reLogin(model: UserForLoginParam) {
    this.snotifyService.confirm(
      this.translateService.instant('System.Message.ConfirmReLogin'),
      this.translateService.instant('System.Caption.Warning'),
      async () => {
        model.confirmReLogin = true;
        await firstValueFrom(this.login(model));
      },
      () => this.disLoginEmitter.emit(false)
    )
  }

  setLocalStore(res: LoginResponse) {
    localStorage.setItem(LocalStorageConstants.TOKEN, res.token);
    localStorage.setItem(LocalStorageConstants.USER, JSON.stringify(res.user));
    this.decodedToken = this.jwtHelper.decodeToken(res.token);
    this.currentUser = res.user.user;
  }

  loggedIn(): boolean {
    const token: string = localStorage.getItem(LocalStorageConstants.TOKEN);
    const curentUser: Users = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
    if (!curentUser || !curentUser.roles) {
      return false;
    }

    return !this.jwtHelper.isTokenExpired(token);
  }

  logout(model: UserForLoginParam, callLogout: boolean = true, message: string = ''): Observable<object> {
    localStorage.removeItem(LocalStorageConstants.TOKEN);
    localStorage.removeItem(LocalStorageConstants.USER);
    this.decodedToken = null;
    if (message) {
      if (message == 'Logout') {
        this.snotifyService.success(
          this.translateService.instant(`System.Message.${message}`),
          this.translateService.instant('System.Caption.Success'));
      } else {
        this.snotifyService.warning(
          this.translateService.instant(`System.Message.${message}`),
          this.translateService.instant('System.Caption.Success'));
      }
    }
    this.router.navigate(['/login']);
    return callLogout ? this.http.post(this.baseUrl + 'Auth/Logout', model) : of(null);
  }

  loginExpired(model: UserForLoginParam, isMessage: boolean = true): Observable<object> {
    localStorage.removeItem(LocalStorageConstants.TOKEN);
    localStorage.removeItem(LocalStorageConstants.USER);
    this.decodedToken = null;
    if (isMessage) {
      this.snotifyService.warning(
        this.translateService.instant('System.Message.LoginExpired'),
        this.translateService.instant('System.Caption.Warning'));
    }
    this.router.navigate(['/login']);
    return this.http.post(this.baseUrl + 'Auth/Logout', model);
  }

  countLeaveEdit(userID: number): Observable<number> {
    var params = new HttpParams().appendAll({ userID });
    return this.http.get<number>(this.baseUrl + 'Auth/CountLeaveEdit', { params });
  }

  countSeaHrEdit(): Observable<number> {
    return this.http.get<number>(this.baseUrl + 'Auth/CountSeaHrEdit');
  }

  countSeaHrConfirm(): Observable<number> {
    return this.http.get<number>(this.baseUrl + 'Auth/CountSeaHrConfirm');
  }

  changePassword(user: UserForLoginParam): Observable<OperationResult> {
    return this.http.post<OperationResult>(this.baseUrl + 'Auth/ChangePassword', user);
  }

  async preloadData() {
    const user = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
    const roles = user?.roles?.find(role => role.roleSym === RoleConstants.DASHBOARD_SEAHR)?.subRoles || [];
    return roles;
  }
}
