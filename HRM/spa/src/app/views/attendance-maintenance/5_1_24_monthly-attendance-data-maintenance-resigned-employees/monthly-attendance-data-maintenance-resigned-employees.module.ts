import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MonthlyAttendanceDataMaintenanceResignedEmployeesRoutingModule } from './monthly-attendance-data-maintenance-resigned-employees-routing.module';
import { MainComponent } from './main/main.component';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { DragScrollComponent } from 'ngx-drag-scroll';
import { FormComponent } from './form/form.component';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';


@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    MonthlyAttendanceDataMaintenanceResignedEmployeesRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    DragScrollComponent,
    NgxMaskDirective,
    NgxMaskPipe,
    TypeaheadModule.forRoot(),
  ]
})
export class MonthlyAttendanceDataMaintenanceResignedEmployeesModule { }
