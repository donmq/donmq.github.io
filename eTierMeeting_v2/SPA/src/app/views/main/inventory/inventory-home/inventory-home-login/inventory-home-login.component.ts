import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { User, UserForLoginDto } from '../../../../../_core/_models/user';
import { AlertifyService } from '../../../../../_core/_services/alertify.service';
import { AuthService } from '../../../../../_core/_services/auth.service';


@Component({
  selector: 'app-inventory-home-login',
  templateUrl: './inventory-home-login.component.html',
  styleUrls: ['./inventory-home-login.component.scss']
})
export class InventoryHomeLoginComponent implements OnInit {
  @Output() check = new EventEmitter<boolean>();
  @ViewChild('childModalLogin', { static: false }) childModalLogin: ModalDirective;

  constructor(
    private _authService: AuthService,
    private _alertifyService: AlertifyService,
    private translate: TranslateService
  ) { }

  dataUser: User;
  user: UserForLoginDto = {
    password: '',
    username: '',
  };

  showChildLogin(): void {
    this.childModalLogin.show();
  }

  hideChildLogin(): void {
    this.childModalLogin.hide();
  }

  ngOnInit(): void {
  }

  login() {
    this.dataUser = JSON.parse(localStorage.getItem('user'));
    if (this.user.username === '') {
      this._alertifyService.error(this.translate.instant('alert.alert_please_enter_your_username'));
      return;
    }
    if (this.user.password === '') {
      this._alertifyService.error(this.translate.instant('alert.alert_please_enter_your_password'));
      return;
    }
    if (this.user.username !== this.dataUser.userName) {
      this._alertifyService.error(this.translate.instant('alert.alert_account_not_the_same'));
      return;
    }
    this._authService.login(this.user).subscribe(
      () => {
        this.childModalLogin.hide();
        this.check.emit(true);
        this.clearLogin();
      },
      error => {
        this._alertifyService.error(this.translate.instant('alert.alert_account_not_found'));
      }
    );
  }

  clearLogin() {
    this.user.username = '';
    this.user.password = '';
  }
}

