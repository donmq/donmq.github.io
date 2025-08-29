import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MonthlyWorkingHoursLeaveHoursReportRoutingModule } from './monthly-working-hours-leave-hours-report-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { MainComponent } from './main/main.component';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    MonthlyWorkingHoursLeaveHoursReportRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    ButtonsModule.forRoot()
  ]
})
export class MonthlyWorkingHoursLeaveHoursReportModule { }
