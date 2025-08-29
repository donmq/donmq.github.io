import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MainComponent } from './main/main.component';
import { dailySwipeResolver } from '@services/attendance-maintenance/s_5_2_5_daily_swipe_card_anomalies_list.service';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {

      dataResolved: dailySwipeResolver
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DailySwipeCardAnomaliesListRoutingModule { }
