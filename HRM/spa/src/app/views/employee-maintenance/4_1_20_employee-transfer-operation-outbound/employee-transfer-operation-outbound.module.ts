import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { EmployeeTransferOperationOutboundRoutingModule } from './employee-transfer-operation-outbound-routing.module';
import { FormComponent } from './form/form.component';
import { MainComponent } from './main/main.component';
import { DragScrollComponent, DragScrollItemDirective } from 'ngx-drag-scroll';


@NgModule({
  declarations: [MainComponent, FormComponent],
  imports: [
    CommonModule,
    EmployeeTransferOperationOutboundRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    TypeaheadModule.forRoot(),
    DragScrollComponent,
    DragScrollItemDirective,
  ]
})
export class EmployeeTransferOperationOutboundModule { }
