import { Component, OnInit } from '@angular/core';
import {
  Router,
  Event as RouterEvent,
  NavigationEnd,
  NavigationStart,
  NavigationCancel,
  NavigationError
} from '@angular/router';
import { LoginDetectHubService } from '@services/login-detect-hub.service';
import { NgxSpinnerService } from 'ngx-spinner';
import '../app/_core/utilities/extension-methods';
import { Title } from '@angular/platform-browser';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { HostApplicationLifetimeHubService } from '@services/host-application-lifetime-hub.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = '';
  factory: string = localStorage.getItem(LocalStorageConstants.FACTORY);
  // Don't remove LoginDetectHubService, because of its constructor
  constructor(
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private loginDetectHubService: LoginDetectHubService,
    private hostApplicationLifetimeHubService: HostApplicationLifetimeHubService,
    private titleService: Title) {
    this.title = `Webleave - ${this.factory}`;

    titleService.setTitle(this.title);
    hostApplicationLifetimeHubService.onApplicationStart();
  }

  ngOnInit(): void {
    this.router.events.subscribe((evt) => {
      this.navigationInterceptor(evt);
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });
  }

  navigationInterceptor(e: RouterEvent) {
    if (e instanceof NavigationStart) {
      this.spinnerService.show();
    }

    if (e instanceof NavigationEnd || e instanceof NavigationCancel || e instanceof NavigationError) {
      this.spinnerService.hide();
    }
  }
}
