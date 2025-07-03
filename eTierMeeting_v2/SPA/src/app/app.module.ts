import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar-next';
import { IconSetService } from '@coreui/icons-angular';
import { SnotifyModule, SnotifyService, ToastDefaults } from 'ng-alt-snotify';
import { AppComponent } from './app.component';

// Import containers
import { DefaultLayoutComponent } from './containers';
import { Layout2ForMaintainpageComponent } from './containers/layout2-for-maintainpage/layout2-for-maintainpage.component';
import { P404Component } from './views/error/404.component';
import { P500Component } from './views/error/500.component';
import { LoginComponent } from './views/login/login.component';

// import { RegisterComponent } from './views/register/register.component';

const APP_CONTAINERS = [
  DefaultLayoutComponent,
  Layout2ForMaintainpageComponent
];

import {
  AppAsideModule,
  AppBreadcrumbModule,
  AppHeaderModule,
  AppFooterModule,
  AppSidebarModule,
} from '@coreui/angular';

// Import routing module
import { AppRoutingModule } from './app.routing';

// Import 3rd party components
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgChartsConfiguration, NgChartsModule } from 'ng2-charts';
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { PaginationModule } from "ngx-bootstrap/pagination";
import { NgxSpinnerModule } from "ngx-spinner";
import { FormsModule } from "@angular/forms";
import { ModalModule } from "ngx-bootstrap/modal";
import { AuthService } from "./_core/_services/auth.service";
import { AuthGuard } from './auth/auth.guard';
import { JwtModule } from "@auth0/angular-jwt";
import { UnauthorizedInterceptor } from './_core/_helpers/unauthorized.interceptor';
import { PageSettingsGuard } from './_core/_guards/page-settings.guard';
import { PageInitGuard } from './_core/_guards/page-init.guard';
import { appInitializer } from './_core/_services/app-init.service';
import { CommonService } from './_core/_services/common.service';
import { environment } from '../environments/environment';
import { PageItemSettingGuard } from './_core/_guards/production/T2/CTB/page-item-setting.guard';
import { EfficiencyGuard } from './_core/_guards/production/T5/efficiency.guard';
import { LocalStorageConstants } from '@constants/storage.constants';

export function tokenGetter() {
  return localStorage.getItem(LocalStorageConstants.TOKEN);
}

@NgModule({
  imports: [
    HttpClientModule,
    FormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AppAsideModule,
    AppBreadcrumbModule.forRoot(),
    AppFooterModule,
    NgxSpinnerModule,
    AppHeaderModule,
    AppSidebarModule,
    PerfectScrollbarModule,
    PaginationModule.forRoot(),
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    ModalModule.forRoot(),
    NgChartsModule,
    SnotifyModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: [environment.allowedDomains],
        disallowedRoutes: [environment.disallowedRoutes]
      },
    }),
  ],
  declarations: [
    AppComponent,
    ...APP_CONTAINERS,
    P404Component,
    P500Component,
    LoginComponent,
    Layout2ForMaintainpageComponent
  ],
  providers: [
    { provide: NgChartsConfiguration, useValue: { generateColors: false }},
    {
      provide: 'SnotifyToastConfig',
      useValue: ToastDefaults
    },
    SnotifyService,
    AuthService,
    AuthGuard,
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy
    },
    IconSetService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: UnauthorizedInterceptor,
      multi: true
    },
    PageSettingsGuard,
    PageInitGuard,
    {
      provide: APP_INITIALIZER,
      useFactory: appInitializer,
      multi: true,
      deps: [CommonService]
    },
    PageItemSettingGuard,
    EfficiencyGuard
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
