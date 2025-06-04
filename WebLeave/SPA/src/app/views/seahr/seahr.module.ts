import { CommonModule } from '@angular/common';
import { SeaHRRoutingModule } from './seahr-routing.module';
import { SeahrDashboardComponent } from './seahr-dashboard/seahr-dashboard.component';
import { SeahrEmpManagementComponent } from './seahr-emp-management/seahr-emp-management.component';
import { SeaConfirmResolver } from '@resolvers/seahr/sea-confirm.resolver';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerModule } from 'ngx-spinner';
import { HttpClientModule } from '@angular/common/http';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ViewConfirmDailyMainComponent } from './view-confirm-daily/view-confirm-daily.component';
import { SeahrHistoryComponent } from './seahr-history/seahr-history.component';
import { SeahrAddManuallyComponent } from './seahr-add-manually/seahr-add-manually.component';
import { SeahrDeleteComponent } from './seahr-delete/seahr-delete.component';
import { SeahrAddComponent } from './seahr-add/seahr-add.component';
import { SeaEditLeaveResolver } from '@resolvers/seahr/sea-edit-leave.resolver';
import { ManageCommentArchiveComponent } from './manage-comment-archive/manage-comment-archive.component';
import { CommonsModule } from '../commons/commons.module';
import { ComponentGuard } from '@guards/component.guard';
import { A2Edatetimepicker } from 'ng2-eonasdan-datetimepicker';
import { SeahrPermissionRightsComponent } from './seahr-permission-rights/seahr-permission-rights.component';
import { TooltipDirective } from '@directives/tooltip.directive';

@NgModule({
  declarations: [
    SeahrDashboardComponent,
    SeahrAddComponent,
    SeahrDeleteComponent,
    SeahrEmpManagementComponent,
    ViewConfirmDailyMainComponent,
    SeahrHistoryComponent,
    SeahrAddManuallyComponent,
    ManageCommentArchiveComponent,
    SeahrPermissionRightsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CommonsModule,
    HttpClientModule,
    A2Edatetimepicker,
    ModalModule.forRoot(),
    SeaHRRoutingModule,
    TranslateModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TooltipDirective,
    NgxSpinnerModule,
    NgSelectModule,
  ],
  providers: [
    SeaConfirmResolver,
    SeaEditLeaveResolver,
    ComponentGuard
  ]
})
export class SeaHRModule { }
