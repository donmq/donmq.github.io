import { Component, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { ModalDirective } from "ngx-bootstrap/modal";
import { NgxSpinnerService } from "ngx-spinner";
import { AuthService } from "../../_core/_services/auth.service";
import { NgSnotifyService } from "../../_core/_services/ng-snotify.service";
import { UserService } from "../../_core/_services/user.service";
import { NavItem } from "../../_nav";
import { LocalStorageConstants } from "@constants/storage.constants";

@Component({
  selector: 'app-dashboard',
  templateUrl: "./layout2-for-maintainpage.component.html",
})
export class Layout2ForMaintainpageComponent {
  public sidebarMinimized = false;
  public navItems = [];
  currentUser: any = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
  oldPassword: string;
  newPassword: string;
  confirmPassword: string;
  @ViewChild("modalChangePassword", { static: false })
  modalEditUser: ModalDirective;

  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }

  constructor(
    private authService: AuthService,
    private snotifyService: NgSnotifyService,
    private router: Router,
    private userService: UserService,
    private spinnerService: NgxSpinnerService,
    private nav: NavItem
  ) {
    // 權限載入菜單
    this.navItems = this.nav.getNav(this.currentUser);
  }

  logout() {
    localStorage.removeItem(LocalStorageConstants.TOKEN);
    localStorage.removeItem(LocalStorageConstants.USER);
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    this.snotifyService.info("Logged out");
    this.router.navigate(["/dashboard"]);
  }

  changePassword() {
    if (this.newPassword !== this.confirmPassword) {
      this.snotifyService.error("Confirm password not match!");
      return;
    }
    this.spinnerService.show();
    this.userService
      .changePassword(
        this.currentUser.username,
        this.oldPassword,
        this.newPassword
      )
      .subscribe(
        (res) => {
          if (res.success) {
            this.snotifyService.success(res.message);
            this.spinnerService.hide();
            this.modalEditUser.hide();
          } else {
            this.snotifyService.error(res.message);
            this.spinnerService.hide();
          }
        },
        (error) => {
          this.snotifyService.error("Fail change pasword user!");
          this.spinnerService.hide();
        }
      );
  }

  goToLayout1() {
    this.router.navigate(['/dashboard']);
  }
}
