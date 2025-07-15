import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { AppComponent } from './app.component';
import { AdminLayoutComponent, DefaultLayoutComponent } from './containers';
import {
  AppAsideModule,
  AppBreadcrumbModule,
  AppHeaderModule,
  AppFooterModule,
  AppSidebarModule,
} from '@coreui/angular';
import { AppRoutingModule } from './app.routing';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ChartsModule } from 'ng2-charts';
import { NgSelect2Module } from 'ng-select2';
import { NgxSpinnerModule } from 'ngx-spinner';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { LoginHomeComponent } from './containers/default-layout/login-home/login-home.component';
import { ChangePasswordComponent } from './containers/default-layout/change-password/change-password.component';
import { MomentModule } from 'angular2-moment';
import { NgIdleKeepaliveModule } from '@ng-idle/keepalive';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { appInitializer } from './_core/_utility/app-initializer';
import { AuthService } from './_core/_services/auth.service';
import { environment } from '../environments/environment';

// Import service
export function tokenGetter() {
  return localStorage.getItem('token');
}
@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AppAsideModule,
    AppBreadcrumbModule.forRoot(),
    AppFooterModule,
    AppHeaderModule,
    AppSidebarModule,
    PerfectScrollbarModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    ChartsModule,
    NgSelect2Module,
    NgxSpinnerModule,
    HttpClientModule,
    ModalModule.forRoot(),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: environment.allowedDomains,
        disallowedRoutes: environment.disallowedRoutes,
      }
    }),
    FormsModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: httpTranslateLoader,
        deps: [HttpClient]
      },
    }),
    ReactiveFormsModule,
    MomentModule,
    NgIdleKeepaliveModule.forRoot(),
    TooltipModule.forRoot()
  ],
  declarations: [
    AppComponent,
    DefaultLayoutComponent,
    AdminLayoutComponent,
    LoginHomeComponent,
    ChangePasswordComponent,
  ],
  providers: [
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy
    },
    {
      provide: APP_INITIALIZER,
      useFactory: appInitializer,
      multi: true,
      deps: [AuthService]
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function httpTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
