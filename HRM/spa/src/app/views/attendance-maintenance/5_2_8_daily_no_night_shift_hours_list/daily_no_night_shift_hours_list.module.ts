import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DailyNoNightShiftHoursListRoutingModule } from './daily_no_night_shift_hours_list-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { MainComponent } from './main/main.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';


@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    DailyNoNightShiftHoursListRoutingModule,
    FormsModule,
    TranslateModule,
    BsDatepickerModule.forRoot(),
    NgSelectModule,
    ReactiveFormsModule
  ]
})
export class DailyNoNightShiftHoursListModule { }
