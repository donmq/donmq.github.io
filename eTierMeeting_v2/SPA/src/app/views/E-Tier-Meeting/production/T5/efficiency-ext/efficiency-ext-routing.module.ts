import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EfficiencymainExtComponent } from './efficiencymain-ext/efficiencymain-ext.component';

const routes: Routes = [
  {
    path: 'efficiencyadidas',
    component: EfficiencymainExtComponent,
    data: {
      title: 'Efficiency-Ext-MainPage'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EfficiencyExtRoutingModule { }
