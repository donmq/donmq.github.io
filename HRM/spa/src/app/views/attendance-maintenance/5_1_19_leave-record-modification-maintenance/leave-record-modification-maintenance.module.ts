import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { MainComponent } from './main/main.component';
import { DragScrollComponent, DragScrollItemDirective } from 'ngx-drag-scroll';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { NgxSpinnerModule } from 'ngx-spinner';
import { PipesModule } from 'src/app/_core/pipes/pipes.module';
import { FormComponent } from './form/form.component';
import { LeaveRecordModificationMaintenanceRoutingModule } from './leave-record-modification-maintenance-routing.module'
import { ModalComponent } from './modal/modal.component';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';

@NgModule({
  declarations: [MainComponent, FormComponent, ModalComponent],
  imports: [
    CommonModule,
    LeaveRecordModificationMaintenanceRoutingModule,
    TranslateModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    DragScrollComponent,
    DragScrollItemDirective,
    PipesModule,
    NgxSpinnerModule,
    ModalModule.forRoot(),
    TypeaheadModule.forRoot(),
    NgxMaskDirective,
    NgxMaskPipe
  ]
})
export class LeaveRecordModificationMaintenanceModule { }
