import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SeaEditLeaveResolver } from '@resolvers/seahr/sea-edit-leave.resolver';
import { SeahrAddManuallyComponent } from './seahr-add-manually/seahr-add-manually.component';
import { SeahrAddComponent } from './seahr-add/seahr-add.component';
import { SeahrDashboardComponent } from './seahr-dashboard/seahr-dashboard.component';
import { SeahrDeleteComponent } from './seahr-delete/seahr-delete.component';
import { SeahrEmpManagementComponent } from './seahr-emp-management/seahr-emp-management.component';
import { SeahrHistoryComponent } from './seahr-history/seahr-history.component';
import { ViewConfirmDailyMainComponent } from './view-confirm-daily/view-confirm-daily.component';
import { SeaConfirmResolver } from '@resolvers/seahr/sea-confirm.resolver';
import { ManageCommentArchiveComponent } from './manage-comment-archive/manage-comment-archive.component';
import { ComponentGuard } from '@guards/component.guard';
import { RoleConstants } from '@constants/role.constants';
import { SeahrPermissionRightsComponent } from './seahr-permission-rights/seahr-permission-rights.component';
import { SeaHrDataResolver } from '@resolvers/seahr/sea-dasboard.resolver';

const routes: Routes = [
  {
    path: '',
    component: SeahrDashboardComponent,
    data: {
      breadcrumb: ''
    },
    resolve: {
      data: SeaHrDataResolver,
    },
  },
  {
    path: 'new-employee',
    component: SeahrAddComponent,
    canLoad: [ComponentGuard],
    data: {
      breadcrumb: 'NewEmployee',
      role: RoleConstants.SEAHR_NEW_EMPLOYEE
    },
  },
  {
    path: 'delete-employee',
    component: SeahrDeleteComponent,
    canLoad: [ComponentGuard],
    data: {
      breadcrumb: 'DeleteEmployee',
      role: RoleConstants.SEAHR_DELETE_EMPLOYEE
    },
  },
  {
    path: 'emp-management',
    component: SeahrEmpManagementComponent,
    canLoad: [ComponentGuard],
    data: {
      breadcrumb: 'EmpManagement',
      role: RoleConstants.SEAHR_EMP_MANAGEMENT
    },
  },

  {
    path: 'sea-confirm',
    canLoad: [ComponentGuard],
    resolve: { res: SeaConfirmResolver },
    loadChildren: () => import('./seahr-confirm/seahr-confirm.module').then(m => m.SeahrConfirmModule),
    data: {
      breadcrumb: 'SeaConfirm',
      role: RoleConstants.SEAHR_SEA_CONFIRM
    }
  },
  {
    canLoad: [ComponentGuard],
    path: 'edit-leave',
    loadChildren: () => import('./seahr-edit-leave/seahr-edit-leave.module').then(m => m.SeahrEditLeaveModule),
    resolve: { res: SeaEditLeaveResolver },
    data: {
      breadcrumb: 'SeaEditLeave',
      role: RoleConstants.SEAHR_EDIT_LEAVE
    }
  },
  {
    canLoad: [ComponentGuard],
    path: 'view-data',
    component: ViewConfirmDailyMainComponent,
    data: {
      breadcrumb: 'ViewData',
      role: RoleConstants.SEAHR_VIEW_DATA
    }
  },
  {
    path: 'export-hp',
    canLoad: [ComponentGuard],
    loadChildren: () => import('./export-hp/export-hp.module').then(m => m.ExportHpModule),
    data: {
      breadcrumb: 'ExportHP',
      role: RoleConstants.SEAHR_EXPORT_HP
    }
  },
  {
    path: 'history',
    canLoad: [ComponentGuard],
    component: SeahrHistoryComponent,
    data: {
      breadcrumb: 'History',
      role: RoleConstants.SEAHR_HISTORY
    },
  },
  {
    path: 'add-manually',
    component: SeahrAddManuallyComponent,
    canLoad: [ComponentGuard],
    data: {
      breadcrumb: 'SeaAddManually',
      role: RoleConstants.SEAHR_ADD_MANUALLY
    }
  },
  {

    path: 'manage-comment-archive',
    component: ManageCommentArchiveComponent,
    canLoad: [ComponentGuard],
    data: {
      breadcrumb: 'ManagerCommentArchive',
      role: RoleConstants.SEAHR_MANAGE_COMMENT_ARCHIVE
    },
  },
  {

    path: 'permission-rights',
    component: SeahrPermissionRightsComponent,
    canLoad: [ComponentGuard],
    data: {
      breadcrumb: 'PermissionRights',
      role: RoleConstants.SEAHR_PERMISSION_RIGHTS
    },
  },
  {
    path: 'allow-leave-sunday',
    canLoad: [ComponentGuard],
    loadChildren: () => import('./allow-leave-sunday/allow-leave-sunday.module').then(m => m.AllowLeaveSundayModule),
    data: {
      breadcrumb: 'AllowLeaveSunday',
      role: RoleConstants.SEAHR_ALLOW_LEAVE_SUNDAY
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SeaHRRoutingModule { }
