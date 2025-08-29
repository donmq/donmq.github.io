import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmployeeLunchBreakTimeSettingRoutingModule } from './employee-lunch-break-time-setting-routing.module';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { MainComponent } from './main/main.component';


@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    EmployeeLunchBreakTimeSettingRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    TranslateModule,
    NgSelectModule,
  ]
})
export class EmployeeLunchBreakTimeSettingModule { }
