import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DepartmentRoutingModule } from './department-routing.module';
import { DepartmentMainComponent } from './department-main/department-main.component';
import { DepartmentAddComponent } from './department-add/department-add.component';
import { DepartmentUpdateComponent } from './department-update/department-update.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TranslateModule } from '@ngx-translate/core';
@NgModule({
  declarations: [
    DepartmentMainComponent,
    DepartmentAddComponent,
    DepartmentUpdateComponent
  ],
  imports: [
    CommonModule,
    DepartmentRoutingModule,
    NgSelectModule,
    FormsModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot(),
    TranslateModule,
  ]
})
export class DepartmentModule { }
