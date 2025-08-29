import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { resolverFactories } from '@services/attendance-maintenance/s_5_1_14_employee-daily-attendance-data-generation.service';
import { MainComponent } from './main/main.component';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {
      resolverFactories: resolverFactories,
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeDailyAttendanceDataGenerationRoutingModule { }
