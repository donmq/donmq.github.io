import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminUserGuard } from '../../_core/_gurad/admin-user.guard';
import { UserGuard } from '../../_core/_gurad/user.guard';
import { AdminHomeComponent } from './admin-home/admin-home.component';
import { BuildingHomeComponent } from './building/building-home/building-home.component';
import { CellHomeComponent } from './cell/cell-home/cell-home.component';
import { CellplnoHomeComponent } from './cellplno/cellplno-home/cellplno-home.component';
import { EmployyeeHomeComponent } from './employee/employyee-home/employyee-home.component';
import { ErrorAdminComponent } from './error-admin/error-admin.component';
import { InventoryCalendarComponent } from './inventory-calendar/inventory-calendar.component';
import { LogoutComponent } from './logout/logout.component';
import { PdcHomeComponent } from './pdc/pdc-home/pdc-home.component';
import { PreliminaryHomeComponent } from './preliminary/preliminary-home/preliminary-home.component';
import { UserHomeComponent } from './user/user-home/user-home.component';

const routes: Routes = [
  {
    path: '',
    component: AdminHomeComponent,
    data: {
      title: 'Admin'
    },
    canActivate: [UserGuard]
  },
  {
    path: '',
    data: {
      title: 'Admin'
    },
    children: [
      {
        path: 'building',
        component: BuildingHomeComponent,
        data: {
          title: 'Building'
        },
        canActivate: [UserGuard]
      },
      {
        path: 'cell',
        component: CellHomeComponent,
        data: {
          title: 'Cell'
        },
        canActivate: [UserGuard]
      },
      {
        path: 'cellplno',
        component: CellplnoHomeComponent,
        data: {
          title: 'Cell_plno'
        },
        canActivate: [UserGuard]
      },
      {
        path: 'employee',
        component: EmployyeeHomeComponent,
        data: {
          title: 'Employee'
        },
        canActivate: [UserGuard]
      },
      {
        path: 'inventory',
        component: InventoryCalendarComponent,
        data: {
          title: 'Inventory'
        },
        canActivate: [UserGuard]
      },
      {
        path: 'pdc',
        component: PdcHomeComponent,
        data: {
          title: 'PDC'
        },
        canActivate: [UserGuard]
      },
      {
        path: 'user',
        component: UserHomeComponent,
        data: {
          title: 'User'
        },
        canActivate: [UserGuard, AdminUserGuard],

      },
      {
        path: 'preliminary',
        component: PreliminaryHomeComponent,
        data: {
          title: 'Preliminary'
        },
        canActivate: [UserGuard, AdminUserGuard],

      },
      {
        path: 'erroradmin',
        component: ErrorAdminComponent,
      },
    ]
  },
  {
    path: 'logout',
    component: LogoutComponent,
    data: {
      title: 'Logout'
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }

