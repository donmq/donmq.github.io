import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoanedMonthAttendanceDataGenerationRoutingModule } from './loaned-month-attendance-data-generation-routing.module';
import { MainComponent } from './main/main.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TranslateModule } from '@ngx-translate/core';
import { SharedModule } from '@views/_shared/shared.module';


@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    LoanedMonthAttendanceDataGenerationRoutingModule,
    FormsModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    TranslateModule,
    ReactiveFormsModule,
    SharedModule
  ]
})
export class LoanedMonthAttendanceDataGenerationModule { }
