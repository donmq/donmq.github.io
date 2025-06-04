import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LeaveHistoryResolver } from '@resolvers/leave/leave-history.resolver';
import { CommonsModule } from '../commons/commons.module';
import { LeaveApproveComponent } from './leave-approve/leave-approve.component';
import { LeaveDashboardComponent } from './leave-dashboard/leave-dashboard.component';
import { LeaveEditDataDetailComponent } from './leave-edit-data-detail/leave-edit-data-detail.component';
import { LeaveEditDataComponent } from './leave-edit-data/leave-edit-data.component';
import { LeaveHistoryComponent } from './leave-history/leave-history.component';
import { LeaveRoutingModule } from './leave-routing.module';
import { LeaveDetailComponent } from './leave-detail/leave-detail.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { ComponentGuard } from '@guards/component.guard';
import { TooltipDirective } from '@directives/tooltip.directive';
@NgModule({
  imports: [
    CommonModule,
    CommonsModule,
    LeaveRoutingModule,
    TranslateModule,
    FormsModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TooltipDirective,
    NgSelectModule,
    ReactiveFormsModule,
  ],
  exports: [],
  declarations: [
    LeaveDashboardComponent,
    LeaveEditDataComponent,
    LeaveEditDataDetailComponent,
    LeaveDetailComponent,
    LeaveApproveComponent,
    LeaveHistoryComponent,
    LeaveHistoryComponent,
  ],
  providers: [
    LeaveHistoryResolver,
    ComponentGuard
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class LeaveModule { }
