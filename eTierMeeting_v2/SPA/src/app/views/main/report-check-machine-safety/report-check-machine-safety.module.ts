import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReportCheckMachineSafetyRoutingModule } from './report-check-machine-safety-routing.module';
import { MainComponent } from './main/main.component';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ReportInventoryRoutingModule } from '../report-inventory/report-inventory-routing.module';


@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    ReportCheckMachineSafetyRoutingModule,
    FormsModule,
    ReportInventoryRoutingModule,
    NgxSpinnerModule,
    BsDatepickerModule.forRoot(),
    TranslateModule
  ]
})
export class ReportCheckMachineSafetyModule { }
