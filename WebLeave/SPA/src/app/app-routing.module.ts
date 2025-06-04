import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DefaultLayoutComponent } from './containers/default-layout/default-layout.component';
import { AuthGuard } from '@guards/auth/auth.guard';
import { DashboardGuard } from '@guards/dashboard.guard';
import { RoleConstants } from '@constants/role.constants';
import { NoAuthLayout } from './containers/no-auth-layout/no-auth-layout.component';
const routes: Routes = [
  {
    path: 'login',
    loadChildren: () =>
      import('./views/login/login.module').then((m) => m.LoginModule),
  },
  {
    path: 'no-auth',
    component: NoAuthLayout,
    children: [
      {
        path: 'leave/personal-detail/:empNumber',
        data: {
          breadcrumb: 'PersonalDetail',
          role: RoleConstants.LEAVE_PERSONAL_DETAIL
        },
        loadComponent: () => import('./views/leave/personal-detail/personal-detail.component').then(c => c.PersonalDetailComponent)
      }
    ]
  },
  {
    path: '',
    canActivate: [AuthGuard],
    component: DefaultLayoutComponent,
    data: {
      breadcrumb: 'Home',
    },
    runGuardsAndResolvers: 'always',
    children: [
      {
        path: '',
        loadChildren: () =>
          import('./views/dashboard/dashboard.module').then(
            (m) => m.DashboardModule
          ),
        data: {
          breadcrumb: '',
        },
      },
      {
        canLoad: [DashboardGuard],
        path: 'leave',
        loadChildren: () =>
          import('./views/leave/leave.module').then((m) => m.LeaveModule),
        data: {
          breadcrumb: 'Leave',
          role: RoleConstants.DASHBOARD_LEAVE
        },
      },
      {
        canLoad: [DashboardGuard],
        path: 'seahr',
        loadChildren: () =>
          import('./views/seahr/seahr.module').then((m) => m.SeaHRModule),
        data: {
          breadcrumb: 'SeaHR',
          role: RoleConstants.DASHBOARD_SEAHR
        },
      },
      {
        canLoad: [DashboardGuard],
        path: 'report',
        loadChildren: () =>
          import('./views/report/report.module').then((m) => m.ReportModule),
        data: {
          breadcrumb: 'Report',
          role: RoleConstants.DASHBOARD_REPORT
        },
      },
      {
        canLoad: [DashboardGuard],
        path: 'manage',
        loadChildren: () =>
          import('./views/manage/manage.module').then((m) => m.ManageModule),
        data: {
          breadcrumb: 'Manage',
          role: RoleConstants.DASHBOARD_MANAGE
        },
      },
      {
        path: 'about',
        loadChildren: () =>
          import('./views/about/about-routing.module').then(
            (m) => m.AboutRoutingModule
          ),
        data: {
          breadcrumb: 'About',
        },
      },
    ],
  },
  {
    path: '**',
    redirectTo: '',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' })],
  exports: [RouterModule],
})
export class AppRoutingModule { }
