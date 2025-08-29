

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { OrganizationChartResolver } from '@services/organization-management/s_3_1_3_organization-chart.service';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {

      dataResolved: OrganizationChartResolver
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrganizationChartRoutingModule { }
