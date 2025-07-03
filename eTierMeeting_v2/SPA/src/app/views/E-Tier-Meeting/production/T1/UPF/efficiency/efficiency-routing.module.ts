import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EfficiencymainComponent } from './efficiencymain/efficiencymain.component';


const routes: Routes = [
  {
    path: 'efficiencymain/UPF/:deptId',
    component: EfficiencymainComponent,
    data: {
      title: 'Efficiency-MainPage',
      page_Name: 'Efficiency'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EfficiencyRoutingModule { }
