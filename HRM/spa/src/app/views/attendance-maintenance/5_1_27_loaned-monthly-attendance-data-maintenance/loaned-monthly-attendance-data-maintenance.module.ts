import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';

import { LoanedMonthlyAttendanceDataMaintenanceRoutingModule } from './loaned-monthly-attendance-data-maintenance-routing.module';
import { DragScrollComponent } from 'ngx-drag-scroll';


@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    LoanedMonthlyAttendanceDataMaintenanceRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    TypeaheadModule.forRoot(),
    DragScrollComponent,
  ]
})
export class LoanedMonthlyAttendanceDataMaintenanceModule { }
