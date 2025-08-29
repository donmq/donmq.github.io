import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { EmployeeDailyAttendanceDataGenerationRoutingModule } from './employee-daily-attendance-data-generation-routing.module';
import { MainComponent } from './main/main.component';


@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    EmployeeDailyAttendanceDataGenerationRoutingModule,
    FormsModule,
    BsDatepickerModule.forRoot(),
    TranslateModule,
    NgSelectModule
  ]
})
export class EmployeeDailyAttendanceDataGenerationModule { }
