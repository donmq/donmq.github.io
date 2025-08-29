import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MainComponent } from './main/main.component';
import { dailyNoNightResolver } from '@services/attendance-maintenance/s_5_2_8_daily_no_night_shift_hours_list.service';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {

      dataResolved: dailyNoNightResolver
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DailyNoNightShiftHoursListRoutingModule { }
