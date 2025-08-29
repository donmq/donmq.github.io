import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main/main.component';
import { EmployeeTransferHistoryRoutingModule } from './employee-transfer-history-routing.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AddComponent } from './add/add.component';
import { EditComponent } from './edit/edit.component';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { DragScrollComponent } from 'ngx-drag-scroll';
@NgModule({
  imports: [
    CommonModule,
    EmployeeTransferHistoryRoutingModule,
    TranslateModule,
    NgSelectModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    DragScrollComponent,
    FormsModule,
    ModalModule.forRoot(),
    TypeaheadModule.forRoot()
  ],
  declarations: [MainComponent, AddComponent,EditComponent]
})
export class EmployeeTransferHistoryModule { }
