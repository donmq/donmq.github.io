import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageRoleMainComponent } from './manage-role-main/manage-role-main.component';

const routes: Routes = [
  {
    path: '',
    component: ManageRoleMainComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageUserRolesRoutingModule { }
