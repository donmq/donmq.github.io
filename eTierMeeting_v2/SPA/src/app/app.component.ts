import { Component, OnInit } from '@angular/core';
import {
  Router,
  Event as RouterEvent,
  NavigationEnd,
  NavigationStart,
  NavigationCancel,
  NavigationError
} from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from './_core/_services/auth.service';
import { NgxSpinnerService } from 'ngx-spinner';
import './_core/_utilities/extension-methods';
declare var jQuery: any;
import { IconSetService } from '@coreui/icons-angular';
import { freeSet } from '@coreui/icons';
import { LocalStorageConstants } from '@constants/storage.constants';

@Component({
  // tslint:disable-next-line
  selector: 'body',
  template: `
    <router-outlet (activate)="removeNgSelect2()"></router-outlet>
    <ngx-spinner bdColor = "rgba(0, 0, 0, 0.8)" size = "medium" color = "#fff" type = "ball-scale-multiple" [fullScreen] = "true"><p style="color: white" > </p></ngx-spinner>         <ng-snotify></ng-snotify>`,
  providers: [IconSetService],
})
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService();
  constructor(
    private router: Router,
    private authService: AuthService,
    public iconSet: IconSetService,
    private spinnerService: NgxSpinnerService) {
    // iconSet singleton
    iconSet.icons = { ...freeSet };
  }

  ngOnInit() {
    (function ($) { $.fn.select2.defaults.set('theme', 'bootstrap') })(jQuery);
    this.router.events.subscribe((evt) => {
      this.navigationInterceptor(evt)
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });
    const token = localStorage.getItem(LocalStorageConstants.TOKEN);
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
  }

  navigationInterceptor(e: RouterEvent) {
    if (e instanceof NavigationStart) {
      this.spinnerService.show();
    }

    if (e instanceof NavigationEnd || e instanceof NavigationCancel || e instanceof NavigationError) {
      this.spinnerService.hide();
    }
  }

  removeNgSelect2() {
    document.querySelectorAll('.select2-container--open').forEach(el => el.remove());
  }
}
