import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContributeRateSettingRoutingModule } from './contribute-rate-setting-routing.module';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';


@NgModule({
  declarations: [
    MainComponent,
    FormComponent],
  imports: [
    CommonModule,
    ContributeRateSettingRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    NgxMaskDirective, NgxMaskPipe
  ]
})
export class ContributeRateSettingModule { }
