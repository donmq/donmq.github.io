import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FemaleEmployeeMenstrualLeaveHoursMaintenanceRoutingModule } from './female-employee-menstrual-leave-hours-maintenance-routing.module';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { DragScrollComponent } from 'ngx-drag-scroll';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { ModalComponent } from './modal/modal.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';


@NgModule({
  declarations: [
    MainComponent,
    FormComponent,
    ModalComponent
  ],
  imports: [
    CommonModule,
    FemaleEmployeeMenstrualLeaveHoursMaintenanceRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    DragScrollComponent,
    TypeaheadModule.forRoot(),
    ModalModule.forRoot(),
    NgxMaskDirective,
    NgxMaskPipe,
  ]
})
export class FemaleEmployeeMenstrualLeaveHoursMaintenanceModule { }
