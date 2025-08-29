import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { MainComponent } from './main/main.component';
import { IndividualMonthlyWorkingHoursReportRoutingModule } from './individual-monthly-working-hours-report-routing.module';
@NgModule({
  imports: [
    CommonModule,
    TranslateModule,
    IndividualMonthlyWorkingHoursReportRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    BsDatepickerModule.forRoot()
  ],
  declarations: [
    MainComponent
  ],
  exports: []
})
export class IndividualMonthlyWorkingHoursReportModule { }
