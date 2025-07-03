import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { QualityMainComponent } from './quality-main/quality-main.component';

const routes: Routes = [
  {
    path: 'qualitymain/CTB/:deptId',
    component: QualityMainComponent,
    data: {
      title: 'Quality-MainPage',
      page_Name: 'Quality'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class QualityRoutingModule { }
