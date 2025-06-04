import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageDashboardComponent } from './manage-dashboard/manage-dashboard.component';
import { DatepickerManagementComponent } from './datepicker-management/datepicker-management.component';
import { ComponentGuard } from '@guards/component.guard';
import { RoleConstants } from '@constants/role.constants';

const routes: Routes = [
  {
    path: '',
    component: ManageDashboardComponent,
    data: {
      breadcrumb: ''
    }
  },
  {
    path: 'position',
    canLoad: [ComponentGuard],
    loadChildren: () => import('./position-manage/position-manage.module').then(m => m.PositionManageModule),
    data: {
      role: RoleConstants.MANAGE_POSITION
    }
  },
  {
    path: 'company',
    canLoad: [ComponentGuard],
    loadChildren: () => import('./company-manage/company-manage.module').then(m => m.CompanyManageModule),
    data: {
      role: RoleConstants.MANAGE_COMPANY
    }
  },
  {
    canLoad: [ComponentGuard],
    path: 'category',
    loadChildren: () => import('./category-management/category-management.module').then(m => m.CategoryManagementModule),
    data: {
      role: RoleConstants.MANAGE_CATEGORY
    }
  },
  {
    canLoad: [ComponentGuard],
    path: 'group-base',
    loadChildren: () => import('./group-base/group-base.module').then(m => m.GroupBaseModule),
    data: {
      role: RoleConstants.MANAGE_GROUP
    }
  },
  {
    canLoad: [ComponentGuard],
    path: 'holiday',
    loadChildren: () => import('./holiday/holiday.module').then(m => m.HolidayModule),
    data: {
      role: RoleConstants.MANAGE_HOLIDAY
    }
  },
  {
    canLoad: [ComponentGuard],
    path: 'department',
    loadChildren: () => import('./department/department.module').then(m => m.DepartmentModule),
    data: {
      breadcrumb: 'Department',
      role: RoleConstants.MANAGE_DEPARTMENT
    }
  },
  {
    canLoad: [ComponentGuard],
    path: 'datepicker',
    component: DatepickerManagementComponent,
    data: {
      breadcrumb: 'Datepicker',
      role: RoleConstants.MANAGE_DATEPICKER
    }
  },
  {
    path: 'area',
    canLoad: [ComponentGuard],
    loadChildren: () => import('./area-manage/area-manage.module').then(m => m.AreaManageModule),
    data: {
      role: RoleConstants.MANAGE_AREA
    }
  },
  {
    path: 'building',
    canLoad: [ComponentGuard],
    loadChildren: () => import('./building-manage/building-manage.module').then(m => m.BuildingManageModule),
    data: {
      role: RoleConstants.MANAGE_BUILDING
    }
  },
  {
    path: 'part',
    canLoad: [ComponentGuard],
    loadChildren: () => import('./team-management/team-management.module').then(m => m.TeamManagementModule),
    data: {
      role: RoleConstants.MANAGE_TEAM
    }
  },
  {
    canLoad: [ComponentGuard],
    path: 'employee',
    loadChildren: () => import('./employee-manage/employee-manage.module').then(m => m.EmployeeManageModule),
    data: {
      role: RoleConstants.MANAGE_EMPLOYEE
    }
  },
  {
    path: 'user',
    canLoad: [ComponentGuard],
    loadChildren: () => import('./manage-users/manage-users.module').then(m => m.ManageUsersModule),
    data: {
      breadcrumb: 'User',
      role: RoleConstants.MANAGE_USER
    }
  },
  {
    path: 'lunch-break',
    // canLoad: [ComponentGuard],
    loadChildren: () => import('./lunch-break/lunch-break.module').then(m => m.LunchBreakModule),
    data: {
      breadcrumb: 'LunchBreak',
      role: RoleConstants.MANAGE_LUNCHBREAK
    }
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageRoutingModule { }
