import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { overtimeApplicationMaintenanceResolver } from '@services/attendance-maintenance/s_5_1_12_overtime-application-maintenance.service';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {
      dataResolved: overtimeApplicationMaintenanceResolver
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
export class OvertimeApplicationMaintenanceRoutingModule { }
