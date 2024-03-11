import { DefaultLayoutComponent } from './containers/default-layout/default-layout.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full',
  },
  {
    path: '',
    // canActivate: [AuthGuard],
    component: DefaultLayoutComponent,
    data: {
      title: 'Home'
    },
    // children: [
    //   {
    //     path: 'dashboard',
    //     component: DashboardComponent
    //   },
    //   {
    //     path: 'system-maintenance',
    //     loadChildren: () => import('./views/system-maintenance/system-maintenance.module').then(m => m.SystemMaintenanceModule)
    //   },
    //   {
    //     path: 'basic-maintenance',
    //     loadChildren: () => import('./views/basic-maintenance/basic-maintenance.module').then(m => m.BasicMaintenanceModule)
    //   },
    //   {
    //     path: 'organization-management',
    //     loadChildren: () => import('./views/organization-management/organization-management.module').then(m => m.OrganizationManagementModule)
    //   },
    //   {
    //     path: 'employee-maintenance',
    //     loadChildren: () => import('./views/employee-maintenance/employee-maintenance.module').then(m => m.EmployeeMaintenanceModule)
    //   },
    // ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
