import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageUserRoleResolver } from '@resolvers/manage/manage-user-role.resolver';
import { ManageUserDeleteComponent } from './manage-user-delete/manage-user-delete.component';
import { ManageUserListComponent } from './manage-user-list/manage-user-list.component';

const routes: Routes = [
  {
    path: '',
    component: ManageUserListComponent,
  },
  {
    path: 'delete',
    component: ManageUserDeleteComponent,
    data: {
      breadcrumb: 'Delete'
    }
  },
  {
    path: 'roles/:id',
    loadChildren: () => import('../manage-user-roles/manage-user-roles.module').then(m => m.ManageUserRolesModule),
    resolve: { user: ManageUserRoleResolver },
    data: {
      breadcrumb: 'Roles'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageUsersRoutingModule { }
