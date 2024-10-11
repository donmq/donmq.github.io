
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

import { IconModule, IconSetService } from '@coreui/icons-angular';
// import { IconDirective } from '@coreui/icons-angular';

// const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
//   suppressScrollX: true
// };

import { GlobalHttpInterceptor } from '../app/_core/utilities/global-http-interceptor';
import { AppComponent } from './app.component';
// Import containers
import { DefaultLayoutComponent } from './containers';

const APP_CONTAINERS = [
  DefaultLayoutComponent
];

// import {
//   AppAsideModule,
//   AppBreadcrumbModule,
//   AppFooterModule,
//   AppHeaderModule,
//   AppSidebarModule,
// } from '@coreui/angular';

// Import routing module
import { AppRoutingModule } from './app.routing';

// Import 3rd party components
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
// import { JwtModule } from '@auth0/angular-jwt';
import { NgSelectModule } from '@ng-select/ng-select';
// import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
// import { TranslateHttpLoader } from '@ngx-translate/http-loader';
// import { NgxOrgChartModule } from '@tots/ngx-org-chart';
// import { SnotifyModule, SnotifyService, ToastDefaults } from 'ng-alt-snotify';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxBreadcrumbModule } from "ngx-dynamic-breadcrumb";
// import { IConfig, NgxMaskDirective, NgxMaskPipe, provideNgxMask } from 'ngx-mask';
// import { NgxPanZoomModule } from 'ngx-panzoom';
// import { NgxSpinnerModule } from 'ngx-spinner';
// import { ModalService } from '../app/_core/services/modal.service';
import { environment } from '../environments/environment';
// import { LocalStorageConstants } from './_core/constants/local-storage.constants';
// import { P404Component } from './views/error/404.component';
// import { P500Component } from './views/error/500.component';
// import { LoginComponent } from './views/login/login.component';
// const maskConfig: Partial<IConfig> = {
//   validation: false,
// };

// export function tokenGetter() {
//   return localStorage.getItem(LocalStorageConstants.TOKEN);
// }
// export function HttpLoaderFactory(http: HttpClient) {
//   return new TranslateHttpLoader(http, './assets/i18n/', '.json');
// }
@NgModule({
  imports: [
    HttpClientModule,
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    CommonModule,
    // NgxOrgChartModule,
    // PerfectScrollbarModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    IconModule,
    // SnotifyModule,
    NgSelectModule,
    // NgxSpinnerModule,
    // NgxPanZoomModule,
    NgxBreadcrumbModule.forRoot(),
    // NgxMaskDirective,
    // NgxMaskPipe,
    // TranslateModule.forRoot({
    //   loader: {
    //     provide: TranslateLoader,
    //     useFactory: HttpLoaderFactory,
    //     deps: [HttpClient],
    //   },
    // }),
    // JwtModule.forRoot({
    //   config: {
    //     tokenGetter: tokenGetter,
    //     allowedDomains: environment.allowedDomains,
    //     disallowedRoutes: environment.disallowedRoutes,
    //   },
    // }),
  ],
  declarations: [
    AppComponent,
    ...APP_CONTAINERS,
    // P404Component,
    // P500Component,
    // LoginComponent
  ],
  providers: [
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy
    },
    // { provide: 'SnotifyToastConfig', useValue: ToastDefaults },
    { provide: HTTP_INTERCEPTORS, useClass: GlobalHttpInterceptor, multi: true },
    // SnotifyService,
    IconSetService,
    // ModalService,
    // provideNgxMask(maskConfig),
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

