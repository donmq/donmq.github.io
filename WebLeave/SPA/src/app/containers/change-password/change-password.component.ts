import { Component, OnInit } from '@angular/core';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { Users } from '@models/auth/users.model';
import { UserForLoginParam } from '@params/auth/user-for-login.param';
import { AuthService } from '@services/auth/auth.service';
import { InjectBase } from '@utilities/inject-base-app';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { catchError, firstValueFrom, of, tap } from 'rxjs';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent extends InjectBase implements OnInit {
  param: UserForLoginParam = <UserForLoginParam>{};
  confirmPassword: string = '';

  constructor(
    public bsModalRef: BsModalRef,
    private authService: AuthService) {
    super();
  }

  ngOnInit(): void {
  }

  reset() {
    this.param = <UserForLoginParam>{};
    this.confirmPassword = '';
  }

  async save() {
    // Kiểm tra form nhập có hợp lệ không
    if (!this.param.newPassword || !this.confirmPassword || this.param.newPassword !== this.confirmPassword)
      return this.snotifyService.error(
        this.translateService.instant('System.Message.ConfirmPassword'),
        this.translateService.instant('System.Caption.Error'));

    // Kiểm tra user có hợp lệ không
    const user: Users = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
    if (!user || !user.username)
      return this.snotifyService.error(
        this.translateService.instant('System.Message.AccountNotFound'),
        this.translateService.instant('System.Caption.Error'));

    // Gán username
    this.param.username = user.username;

    // Tiến hành đổi mật khẩu
    this.spinnerService.show();
    await firstValueFrom(this.authService.changePassword(this.param)
      .pipe(
        tap(res => {
          this.spinnerService.hide();
          if (res.isSuccess) {
            this.snotifyService.success(
              this.translateService.instant('System.Message.ChangePasswordOKMsg'),
              this.translateService.instant('System.Caption.Success'));
            this.bsModalRef.hide();
          } else {
            this.snotifyService.error(
              this.translateService.instant('System.Message.ChangePasswordErrorMsg'),
              this.translateService.instant('System.Caption.Error'));
          }
        }),
        catchError(() => {
          this.spinnerService.hide();
          this.snotifyService.error(
            this.translateService.instant('System.Message.LogInFailed'),
            this.translateService.instant('System.Caption.Error'));
          return of(null);
        })
      ));
  }
}
