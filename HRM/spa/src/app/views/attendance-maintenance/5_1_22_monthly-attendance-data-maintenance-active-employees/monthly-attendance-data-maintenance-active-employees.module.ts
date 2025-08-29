import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MonthlyAttendanceDataMaintenanceActiveEmployeesRoutingModule } from './monthly-attendance-data-maintenance-active-employees-routing.module';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { DragScrollComponent, DragScrollItemDirective } from 'ngx-drag-scroll';
import { MainComponent } from './main/main.component';
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
    MonthlyAttendanceDataMaintenanceActiveEmployeesRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    DragScrollComponent,
    DragScrollItemDirective,
    NgxMaskDirective,
    NgxMaskPipe,
    TypeaheadModule.forRoot(),
  ]
})
export class MonthlyAttendanceDataMaintenanceActiveEmployeesModule { }
