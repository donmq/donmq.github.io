import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AttendanceAbnormalityDataMaintenanceRoutingModule } from './attendance-abnormality-data-maintenance-routing.module';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { DragScrollComponent } from 'ngx-drag-scroll';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ModalComponent } from './modal/modal.component';


@NgModule({
  declarations: [
    MainComponent,
    FormComponent,
    ModalComponent
  ],
  imports: [
    CommonModule,
    AttendanceAbnormalityDataMaintenanceRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    DragScrollComponent,
    TypeaheadModule.forRoot(),
    NgxMaskDirective,
    NgxMaskPipe,
    ModalModule.forRoot(),
  ]
})
export class AttendanceAbnormalityDataMaintenanceModule { }
