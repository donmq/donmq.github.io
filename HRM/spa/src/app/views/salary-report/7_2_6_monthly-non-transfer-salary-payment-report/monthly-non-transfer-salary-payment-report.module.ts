import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MonthlyNonTransferSalaryPaymentReportRoutingModule } from './monthly-non-transfer-salary-payment-report-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { DragScrollComponent } from 'ngx-drag-scroll';
import { MainComponent } from './main/main.component';


@NgModule({
  declarations: [
    MainComponent,
  ],
  imports: [
    CommonModule,
    MonthlyNonTransferSalaryPaymentReportRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    NgxMaskDirective, NgxMaskPipe,
    CollapseModule.forRoot(),
    TypeaheadModule.forRoot(),
    DragScrollComponent
  ]
})
export class MonthlyNonTransferSalaryPaymentReportModule { }
