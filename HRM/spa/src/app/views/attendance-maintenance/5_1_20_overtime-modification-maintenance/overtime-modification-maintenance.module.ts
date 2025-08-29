import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { OvertimeModificationMaintenanceRoutingModule } from './overtime-modification-maintenance-routing.module';
import { DragScrollComponent } from 'ngx-drag-scroll';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { ModalComponent } from './modal/modal.component';
import { ModalModule } from 'ngx-bootstrap/modal';
@NgModule({
  imports: [
    CommonModule,
    TranslateModule,
    OvertimeModificationMaintenanceRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    DragScrollComponent,
    TypeaheadModule.forRoot(),
    NgxMaskDirective,
    NgxMaskPipe,
    ModalModule.forRoot()
  ],
  declarations: [
    MainComponent,
    FormComponent,
    ModalComponent
  ],
  exports: []
})
export class OvertimeModificationMaintenanceModule { }
