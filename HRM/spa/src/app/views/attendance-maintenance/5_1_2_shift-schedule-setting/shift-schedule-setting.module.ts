import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { ShiftScheduleSettingRoutingModule } from './shift-schedule-setting-routing.module';

import { FormComponent } from './form/form.component';
import { MainComponent } from "./main/main.component";


@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    ShiftScheduleSettingRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    TimepickerModule.forRoot(),
    TranslateModule,
    NgxMaskDirective,
    NgxMaskPipe,
    NgSelectModule
  ]
})
export class ShiftScheduleSettingModule { }
