import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AnnualOvertimeHoursReportRoutingModule } from './annual-overtime-hours-report-routing.module';
import { MainComponent } from "./main/main.component";
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';


@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    AnnualOvertimeHoursReportRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
  ]
})
export class AnnualOvertimeHoursReportModule { }
