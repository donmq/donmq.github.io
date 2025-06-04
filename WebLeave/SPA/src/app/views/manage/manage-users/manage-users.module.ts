import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from "ngx-bootstrap/pagination";
import { ModalModule } from 'ngx-bootstrap/modal';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ManageUserListComponent } from './manage-user-list/manage-user-list.component';
import { ManageUsersRoutingModule } from './manage-users-routing.module';
import { ManageUserEditComponent } from './manage-user-edit/manage-user-edit.component';
import { ManageUserAddComponent } from './manage-user-add/manage-user-add.component';
import { ManageUserDeleteComponent } from './manage-user-delete/manage-user-delete.component';
import { ManageUserRolesRoutingModule } from '../manage-user-roles/manage-user-roles-routing.module';
import { ManageUserRoleResolver } from '@resolvers/manage/manage-user-role.resolver';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { TooltipDirective } from '@directives/tooltip.directive';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    ManageUsersRoutingModule,
    ManageUserRolesRoutingModule,
    TranslateModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot(),
    TooltipDirective
  ],
  declarations: [
    ManageUserListComponent,
    ManageUserEditComponent,
    ManageUserAddComponent,
    ManageUserDeleteComponent
  ],
  providers: [
    ManageUserRoleResolver
  ]
})
export class ManageUsersModule { }
