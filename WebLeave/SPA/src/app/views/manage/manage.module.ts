import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ManageDashboardComponent } from './manage-dashboard/manage-dashboard.component';
import { ManageRoutingModule } from './manage-routing.module';
import { CommonsModule } from '../commons/commons.module';
import { EmployeeManageModule } from './employee-manage/employee-manage.module';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { DatepickerManagementComponent } from './datepicker-management/datepicker-management.component';
import { ComponentGuard } from '@guards/component.guard';

@NgModule({
  declarations: [
    ManageDashboardComponent,
    DatepickerManagementComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    CommonsModule,
    FormsModule,
    ManageRoutingModule,
    EmployeeManageModule,
    TranslateModule,
    FormsModule,
  ],
  providers: [
    ComponentGuard
  ]
})
export class ManageModule { }
