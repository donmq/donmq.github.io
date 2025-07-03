import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../_core/_services/auth.service";
import { Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { NgSnotifyService } from "../../_core/_services/ng-snotify.service";
import { LocalStorageConstants } from "@constants/storage.constants";

@Component({
  selector: "app-dashboard",
  templateUrl: "login.component.html",
})
export class LoginComponent implements OnInit {
  user: any = {};
  factory: string = localStorage.getItem(LocalStorageConstants.FACTORY);
  backgroundImage: string = `assets/img/background/${this.factory}-background.jpg`;

  constructor(
    private authService: AuthService,
    private router: Router,
    private snotifyService: NgSnotifyService,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit() {
    // if (this.authService.loggedIn) this.router.navigate(["/login"]);
    if (this.authService.loggedIn) this.router.navigate(["/dashboard2"]);
  }

  login() {
    this.spinner.show();
    this.authService.login(this.user).subscribe(
      (next) => {
        this.snotifyService.success("Login Success!!");
        this.spinner.hide();
      },
      (error) => {
        console.log(error);
        this.snotifyService.error("Login failed!!");
        this.spinner.hide();
      },
      () => {
        this.router.navigate(["/dashboard2"]);
        this.spinner.hide();
      }
    );
  }

  backToDashboard() {
    this.router.navigate(["/dashboard"]);
  }
}
