import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DailySwipeCardAnomaliesListRoutingModule } from './daily_swipe_card_anomalies_list-routing.module';
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
    DailySwipeCardAnomaliesListRoutingModule,
    FormsModule,
    TranslateModule,
    BsDatepickerModule.forRoot(),
    NgSelectModule,
    ReactiveFormsModule
  ]
})
export class DailySwipeCardAnomaliesListModule { }
