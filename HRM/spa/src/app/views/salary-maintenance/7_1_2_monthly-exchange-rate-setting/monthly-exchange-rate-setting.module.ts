import { PipesModule } from '../../../_core/pipes/pipes.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { MonthlyExchangeRateSettingRoutingModule } from './monthly-exchange-rate-setting-routing.module';

@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    PipesModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    NgxMaskDirective,
    NgxMaskPipe,
    MonthlyExchangeRateSettingRoutingModule
  ]
})
export class MonthlyExchangeRateSettingModule { }
