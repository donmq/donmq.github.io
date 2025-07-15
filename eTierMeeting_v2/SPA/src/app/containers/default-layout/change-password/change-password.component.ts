import { Component, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ChangePassword } from '../../../_core/_models/change-password';
import { User } from '../../../_core/_models/user';
import { AlertifyService } from '../../../_core/_services/alertify.service';
import { AuthService } from '../../../_core/_services/auth.service';
import { SweetAlertService } from '../../../_core/_services/sweet-alert.service';

@Component({
  selector: 'app-change-pasword',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  @ViewChild('editPass') modelChangePasword: ModalDirective;
  lang: string;

  Password: ChangePassword = {
    id: 0,
    oldPassword: '',
    newPassword: '',
  };

  reNewPassword: string = '';
  dataUser: User;
  
  constructor(
    private _authService: AuthService,
    public translate: TranslateService,
    private _sweetAlert: SweetAlertService,
    private _alertifyService: AlertifyService) { }

  ngOnInit() {
  }
  changePassword() {
    this.dataUser = JSON.parse(localStorage.getItem('user'));
    this.lang = localStorage.getItem('lang');
    if (this.Password.oldPassword === '') {
      return this._sweetAlert.error('Error', this.translate.instant('alert.alert_please_enter_the_old_password'));
    }
    if (this.Password.newPassword === '') {
      return this._sweetAlert.error('Error', this.translate.instant('alert.alert_please_enter_a_new_password'));
    }
    if (this.reNewPassword !== this.Password.newPassword) {
      return this._sweetAlert.error('Error', this.translate.instant('alert.alert_new_password_does_not_match'));
    }
    this.Password.id = this.dataUser.userID;
    this._authService.changePassword(this.Password, this.lang).subscribe(res => {
      if (res.success) {
        this._sweetAlert.success('Success', res.message);
        this.clearForm();
      } else {
        this._sweetAlert.error('Error', res.message);
      }
    }, error => {
      this._alertifyService.error(this.translate.instant('error.system_error'));
    });
  }

  clearForm() {
    this.Password.newPassword = '';
    this.Password.oldPassword = '';
    this.reNewPassword = '';
    this.modelChangePasword.hide();
  }

  showChildLogin(): void {
    this.modelChangePasword.show();
  }

  hideChildLogin(): void {
    this.modelChangePasword.hide();
  }
}
