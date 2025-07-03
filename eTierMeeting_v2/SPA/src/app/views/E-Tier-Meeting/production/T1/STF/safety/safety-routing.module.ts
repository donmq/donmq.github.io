import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SafetymainComponent } from './safetymain/safetymain.component';

const routes: Routes = [
  {
    path: 'safetymain/STF/:deptId',
    component: SafetymainComponent,
    data: {
      title: 'Safety-MainPage',
      page_Name: 'Safety'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SafetyRoutingModule { }
