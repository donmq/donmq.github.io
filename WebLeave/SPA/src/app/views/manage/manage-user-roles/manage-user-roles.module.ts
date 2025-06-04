import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManageUserRolesRoutingModule } from './manage-user-roles-routing.module';
import { ManageRoleAdminComponent } from './manage-role-admin/manage-role-admin.component';
import { ManageRoleGroupComponent } from './manage-role-group/manage-role-group.component';
import { ManageRoleReportComponent } from './manage-role-report/manage-role-report.component';
import { ManageRoleLeaveComponent } from './manage-role-leave/manage-role-leave.component';
import { ManageRoleRankEditComponent } from './manage-role-rank-edit/manage-role-rank-edit.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { TranslateModule } from '@ngx-translate/core';
import { ManageRoleMainComponent } from './manage-role-main/manage-role-main.component';
import { TooltipDirective } from '@directives/tooltip.directive';
import { TreeModule } from 'primeng/tree';

@NgModule({
  imports: [
    CommonModule,
    ManageUserRolesRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    TranslateModule,
    ModalModule.forRoot(),
    ButtonsModule.forRoot(),
    TooltipDirective,
    TreeModule
  ],
  declarations: [
    ManageRoleAdminComponent,
    ManageRoleGroupComponent,
    ManageRoleLeaveComponent,
    ManageRoleReportComponent,
    ManageRoleRankEditComponent,
    ManageRoleMainComponent
  ],
  providers: [
  ],
  schemas: [NO_ERRORS_SCHEMA, NO_ERRORS_SCHEMA]
})
export class ManageUserRolesModule { }
