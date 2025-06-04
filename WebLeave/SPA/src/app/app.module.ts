import { APP_INITIALIZER, CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DefaultLayoutComponent } from './containers/default-layout/default-layout.component';
import { NoAuthLayout } from './containers/no-auth-layout/no-auth-layout.component';
import { SnotifyModule, SnotifyService, ToastDefaults } from 'ng-alt-snotify';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FormsModule } from '@angular/forms';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { ModalModule } from 'ngx-bootstrap/modal';
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}
import { LocalStorageConstants } from './_core/constants/local-storage.enum';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxBreadcrumbModule } from 'ngx-dynamic-breadcrumb';
import { PaginationModule } from 'ngx-bootstrap/pagination';

import { AuthGuard } from './_core/guards/auth/auth.guard';
import { environment } from '@env/environment';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { ChangePasswordComponent } from './containers/change-password/change-password.component';
import { DashboardGuard } from '@guards/dashboard.guard';
import { appInitializer } from '@utilities/app-Initializer';
import { CommonService } from '@services/common/common.service';
import { AuthService } from '@services/auth/auth.service';
import { Router } from '@angular/router';

@NgModule({
  declarations: [AppComponent, DefaultLayoutComponent, NoAuthLayout, ChangePasswordComponent],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    SnotifyModule,
    NgxSpinnerModule,
    FormsModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
      defaultLanguage: 'vi',
    }),
    ModalModule.forRoot(),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: environment.allowedDomains,
        disallowedRoutes: environment.disallowedRoutes,
      },
    }),
    NgxBreadcrumbModule.forRoot(),
    PaginationModule.forRoot(),
  ],
  providers: [
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy,
    },
    { provide: 'SnotifyToastConfig', useValue: ToastDefaults },
    SnotifyService,
    AuthGuard,
    DashboardGuard,
    { provide: APP_INITIALIZER, useFactory: appInitializer, multi: true, deps: [CommonService, AuthService, Router] }
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { }

export function httpTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

export function tokenGetter() {
  return localStorage.getItem(LocalStorageConstants.TOKEN);
}
