import { FormComponent } from './form/form.component';
import { TranslateModule } from '@ngx-translate/core';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main/main.component';
import { AccountAuthorizationSettingRoutingModule } from './account-authorization-setting-routing.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from 'ngx-bootstrap/pagination';
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    PaginationModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    AccountAuthorizationSettingRoutingModule,
  ],
  declarations: [MainComponent, FormComponent],
})
export class AccountAuthorizationSettingModule {}
