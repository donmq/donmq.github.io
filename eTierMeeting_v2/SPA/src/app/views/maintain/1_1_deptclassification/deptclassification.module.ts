import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgSelectModule } from '@ng-select/ng-select';

import { DeptclassificationRoutingModule } from './deptclassification-routing.module';
import { DeptclassificationListComponent } from './deptclassification-list/deptclassification-list.component';
import { DeptclassificationAddComponent } from './deptclassification-add/deptclassification-add.component';
import { DeptclassificationEditComponent } from './deptclassification-edit/deptclassification-edit.component';


@NgModule({
  declarations: [
    DeptclassificationListComponent,
    DeptclassificationAddComponent,
    DeptclassificationEditComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgxSpinnerModule,
    PaginationModule.forRoot(),
    NgSelectModule,
    DeptclassificationRoutingModule
  ]
})
export class DeptclassificationModule { }
