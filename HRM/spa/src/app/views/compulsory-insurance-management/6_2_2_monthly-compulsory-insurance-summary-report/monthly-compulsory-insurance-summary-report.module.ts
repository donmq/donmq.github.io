import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MonthlyCompulsoryInsuranceSummaryReportRoutingModule } from './monthly-compulsory-insurance-summary-report-routing.module';
import { MainComponent } from './main/main.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';

@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    MonthlyCompulsoryInsuranceSummaryReportRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    NgxMaskDirective, NgxMaskPipe
  ]
})
export class MonthlyCompulsoryInsuranceSummaryReportModule { }
