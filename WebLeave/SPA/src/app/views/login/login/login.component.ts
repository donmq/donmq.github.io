import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { catchError, firstValueFrom, of, tap } from 'rxjs';
import { LangConstants } from '@constants/lang.constants';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { KeyValuePair } from '@utilities/key-value-pair';
import { UserForLoginParam } from '@params/auth/user-for-login.param';
import { AuthService } from '@services/auth/auth.service';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent extends InjectBase implements OnInit {
  userForLoginParam: UserForLoginParam = <UserForLoginParam>{
    confirmReLogin: false
  };
  langs: KeyValuePair[] = [
    { key: LangConstants.VN, value: LangConstants.VN.toUpperCase() },
    { key: LangConstants.EN, value: LangConstants.EN.toUpperCase() },
    { key: LangConstants.ZH_TW, value: LangConstants.ZH_TW.toUpperCase() },
  ];
  lang: string = localStorage.getItem(LocalStorageConstants.LANG) || LangConstants.VN;
  factory: string = localStorage.getItem(LocalStorageConstants.FACTORY);
  disLogin: boolean = false;
  @ViewChild('passwordRef') passwordRef: ElementRef<HTMLInputElement>;

  constructor(
    private authService: AuthService,
  ) {
    super();
    this.langChanged();
  }

  ngOnInit() {
    if (this.authService.loggedIn()) {
      this.router.navigate(['/dashboard']);
    }
    this.authService.disLoginEmitter.subscribe(res => {
      this.disLogin = res;
      if (!res)
        this.passwordRef?.nativeElement.focus();
    });
  }

  langChanged() {
    this.lang = this.lang ? this.lang : LangConstants.VN;
    this.translateService.use(this.lang === LangConstants.ZH_TW ? 'zh' : this.lang);
    localStorage.setItem(LocalStorageConstants.LANG, this.lang);
  }

  async login() {
    this.userForLoginParam.ipLocal = localStorage.getItem(LocalStorageConstants.IPLOCAL);
    this.spinnerService.show();
    await firstValueFrom(
      this.authService.login(this.userForLoginParam).pipe(
        tap(() => this.spinnerService.hide()),
        catchError(err => {
          this.spinnerService.hide();
          this.snotifyService.error(
            this.translateService.instant('System.Message.LogInFailed'),
            this.translateService.instant('System.Caption.Error'));
          return of(null);
        })
      )
    );
  }
}
