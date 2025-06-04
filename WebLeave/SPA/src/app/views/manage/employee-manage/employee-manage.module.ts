import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmployeeManageRoutingModule } from './employee-manage-routing.module';
import { EmployeeListComponent } from './employee-list/employee-list.component';
import { EmployeeEditComponent } from './employee-edit/employee-edit.component';
import { EmployeeDetailComponent } from './employee-detail/employee-detail.component';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgSelectModule } from '@ng-select/ng-select';
import { ModalModule } from "ngx-bootstrap/modal";
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TranslateModule } from '@ngx-translate/core';
import { TooltipDirective } from '@directives/tooltip.directive';
@NgModule({
  declarations: [
    EmployeeListComponent,
    EmployeeEditComponent,
    EmployeeDetailComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    EmployeeManageRoutingModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    NgSelectModule,
    TooltipDirective,
    ModalModule.forRoot(),
  ]
})
export class EmployeeManageModule { }
