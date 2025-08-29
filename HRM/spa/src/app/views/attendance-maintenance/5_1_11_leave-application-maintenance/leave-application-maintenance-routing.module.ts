import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { leaveApplicationMaintenanceResolver } from '@services/attendance-maintenance/s_5_1_11_leave-application-maintenance.service';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {
      dataResolved: leaveApplicationMaintenanceResolver
    },
    data: {
      title: 'Main'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LeaveApplicationMaintenanceRoutingModule { }
