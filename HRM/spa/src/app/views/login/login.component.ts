import { Component, OnInit } from "@angular/core";
import { InjectBase } from "@utilities/inject-base-app";
import { CaptionConstants, MessageConstants } from '@constants/message.enum';
import { DirectoryInfomation, ProgramInfomation, UserLoginParam } from '@models/auth/auth';
import { AuthService } from '@services/auth/auth.service';
import { KeyValuePair } from "@utilities/key-value-pair";
import { lastValueFrom } from "rxjs";

@Component({
  selector: "app-dashboard",
  templateUrl: "login.component.html",
  styleUrls: ["./login.component.scss"],
})
export class LoginComponent extends InjectBase implements OnInit {
  user: UserLoginParam = <UserLoginParam>{};
  listFactory: KeyValuePair[] = [];
  constructor(private authService: AuthService) {
    super();
  }

  ngOnInit() {
    if (this.authService.loggedIn)
      this.router.navigate(["/dashboard"])
    this.getListFactory()
  }
  checkEmpty() {
    return this.functionUtility.checkEmpty(this.user.username)
      || this.functionUtility.checkEmpty(this.user.password)
      || this.functionUtility.checkEmpty(this.user.factory)
  }
  login() {
    this.spinnerService.show();
    this.authService.login(this.user).subscribe({
      next: () => {
        this.snotifyService.success(MessageConstants.LOGGED_IN, CaptionConstants.SUCCESS);
        this.spinnerService.hide();
      },
      error: () => {
        this.snotifyService.error(MessageConstants.LOGIN_FAILED, CaptionConstants.ERROR);
        this.spinnerService.hide();
      },
      complete: async () => {
        const password_reset = await lastValueFrom(this.commonService.getPasswordReset());
        if (password_reset) {
          const authProgram = this.commonService.authPrograms
          const user_directory: DirectoryInfomation[] = authProgram.directories || [];
          const roleOfUser: ProgramInfomation[] = authProgram.programs || [];
          const parent = user_directory[1]?.directory_Name.toLowerCase().replace(' ', '-')
          const child = roleOfUser.find(x => x.program_Code == '2.1.8').program_Name.toLowerCase().replace(' ', '-')
          this.snotifyService.clear()
          this.snotifyService.warning(
            this.translateService.instant('System.Message.PasswordReset'),
            this.translateService.instant('System.Caption.Warning'))
          this.router.navigate([`/${parent}/${child}`])
        }
        else this.router.navigate(["/dashboard"]);
      }
    });
  }
  getListFactory() {
    this.spinnerService.show();
    this.authService.getListFactory().subscribe({
      next: res => {
        this.listFactory = res;
        this.spinnerService.hide();
      },
      error: () => {
      }
    })
  }
}
