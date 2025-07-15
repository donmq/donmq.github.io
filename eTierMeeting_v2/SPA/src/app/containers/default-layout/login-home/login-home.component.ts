import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { User, UserForLoginDto } from '../../../_core/_models/user';
import { AlertifyService } from '../../../_core/_services/alertify.service';
import { AuthService } from '../../../_core/_services/auth.service';

@Component({
  selector: 'app-login-home',
  templateUrl: './login-home.component.html',
  styleUrls: ['./login-home.component.scss']
})
export class LoginHomeComponent implements OnInit {
  @ViewChild('lgModal') modelLogin: ModalDirective;
  @Output() showLogin = new EventEmitter<boolean>();
  @Output() dataUser = new EventEmitter<User>();
  showChildLogin(): void {
    this.modelLogin.show();
  }

  hideChildLogin(): void {
    this.modelLogin.hide();
  }
  user: UserForLoginDto = {
    password: '',
    username: '',
  };
  getUrl: Subscription;
  constructor(private _alertifyService: AlertifyService,
    private _authService: AuthService,
    public translate: TranslateService) { }

  ngOnInit() {
  }

  login() {
    if (this.user.username === '') {
      this._alertifyService.error(this.translate.instant('alert.alert_please_enter_your_username'));
      return;
    }
    if (this.user.password === '') {
      this._alertifyService.error(this.translate.instant('alert.alert_please_enter_your_password'));
      return;
    }
    this._authService.login(this.user).subscribe(
      () => {
        this.showLogin.emit(false);
        this.dataUser.emit(JSON.parse(localStorage.getItem('user')));
        this._authService.setTime();
        this.hideChildLogin();
        this.clearForm();
      },
      error => {
        console.log(error)
        this._alertifyService.error(this.translate.instant('alert.alert_account_not_found'));
      }
    );
  }

  clearForm() {
    this.user.password = '';
    this.user.username = '';
  }
}
