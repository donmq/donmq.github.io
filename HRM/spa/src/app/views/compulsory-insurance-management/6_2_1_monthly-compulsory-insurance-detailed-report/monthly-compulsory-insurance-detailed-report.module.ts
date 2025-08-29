import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main/main.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { MonthlyCompulsoryInsuranceDetailedReportRoutingModule } from './monthly-compulsory-insurance-detailed-report-routing.module';


@NgModule({
  declarations: [
    MainComponent],
  imports: [
    CommonModule,
    MonthlyCompulsoryInsuranceDetailedReportRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    NgxMaskDirective, NgxMaskPipe
  ]
})
export class MonthlyCompulsoryInsuranceDetailedReportModule { }
