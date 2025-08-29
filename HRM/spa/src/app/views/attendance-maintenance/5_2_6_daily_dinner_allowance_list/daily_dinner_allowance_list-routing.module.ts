import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MainComponent } from './main/main.component';
import { dailyDinnerResolver } from '@services/attendance-maintenance/s_5_2_6_daily_dinner_allowance_list.service';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {

      dataResolved: dailyDinnerResolver
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DailyDinnerAllowanceListRoutingModule { }
