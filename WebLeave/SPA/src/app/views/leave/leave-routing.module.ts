import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { LeaveDetailResolver } from "@resolvers/leave/leave-detail.resolver";
import { LeaveApproveComponent } from "./leave-approve/leave-approve.component";
import { LeaveHistoryResolver } from "@resolvers/leave/leave-history.resolver";
import { LeaveDashboardComponent } from "./leave-dashboard/leave-dashboard.component";
import { LeaveDetailComponent } from "./leave-detail/leave-detail.component";
import { LeaveEditDataDetailComponent } from "./leave-edit-data-detail/leave-edit-data-detail.component";
import { LeaveEditDataComponent } from "./leave-edit-data/leave-edit-data.component";
import { LeaveHistoryComponent } from "./leave-history/leave-history.component";
import { ComponentGuard } from '@guards/component.guard';
import { RoleConstants } from "@constants/role.constants";

const routes: Routes = [
  {
    path: '',
    component: LeaveDashboardComponent,
    data: {
      breadcrumb: ''
    },
  },
  {
    path: 'history',
    canLoad: [ComponentGuard],
    resolve: { res: LeaveHistoryResolver },
    component: LeaveHistoryComponent,
    data: {
      breadcrumb: 'LeaveHistory',
      role: RoleConstants.LEAVE_HISTORY
    },
  },
  {
    path: 'edit',
    canLoad: [ComponentGuard],
    component: LeaveEditDataComponent,
    data: {
      breadcrumb: 'Edit',
      role: RoleConstants.LEAVE_EDIT_LEAVE
    },
  },
  {
    path: 'requestleavedata',
    component: LeaveEditDataDetailComponent,
    data: {
      breadcrumb: 'RequestLeave'
    }
  },
  {
    path: 'approval',
    canLoad: [ComponentGuard],
    component: LeaveApproveComponent,
    data: {
      breadcrumb: 'Approval',
      role: RoleConstants.LEAVE_APPROVE
    },
  },
  {
    path: 'personal',
    canLoad: [ComponentGuard],
    data: {
      breadcrumb: 'Personal',
      role: RoleConstants.LEAVE_PERSONAL
    },
    loadChildren: () => import('./personal/personal.module').then(m => m.PersonalModule)
  },
  {
    path: 'add',
    canLoad: [ComponentGuard],
    data: {
      breadcrumb: 'Representative',
      role: RoleConstants.LEAVE_REPRESENTATIVE
    },
    loadChildren: () => import('./representative/representative.module').then(m => m.RepresentativeModule)
  },
  {
    path: 'detail/:leaveID',
    resolve: { res: LeaveDetailResolver },
    component: LeaveDetailComponent,
    data: {
      breadcrumb: 'LeaveDetail',
    }
  },
  {
    path: 'surrogate',
    canLoad: [ComponentGuard],
    data: {
      breadcrumb: 'Surrogate',
      role: RoleConstants.LEAVE_SURROGATE
    },
    loadChildren: () => import('./leave-surrogate/leave-surrogate.module').then(m => m.LeaveSurrogateModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LeaveRoutingModule { }
