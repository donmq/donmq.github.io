import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MainComponent } from './main/main.component';
import { dailyUnregisteredResolver } from '@services/attendance-maintenance/s_5_2_7_daily_unregistered_overtime_list.service';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {

      dataResolved: dailyUnregisteredResolver
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DailyUnregisteredOvertimeListRoutingModule { }
