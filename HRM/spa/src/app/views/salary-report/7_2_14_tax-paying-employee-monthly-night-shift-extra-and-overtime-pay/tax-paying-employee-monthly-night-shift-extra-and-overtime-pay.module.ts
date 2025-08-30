import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaxPayingEmployeeMonthlyNightShiftExtraAndOvertimePayRoutingModule } from './tax-paying-employee-monthly-night-shift-extra-and-overtime-pay-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { MainComponent } from './main/main.component';


@NgModule({
  declarations: [
    MainComponent,
  ],
  imports: [
    CommonModule,
    TaxPayingEmployeeMonthlyNightShiftExtraAndOvertimePayRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    NgxMaskDirective, NgxMaskPipe,
  ]
})
export class TaxPayingEmployeeMonthlyNightShiftExtraAndOvertimePayModule { }
