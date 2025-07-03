import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { QualitymainComponent } from './qualitymain/qualitymain.component';

const routes: Routes = [
  {
    path: 'qualitymain/CTB/:deptId',
    component: QualitymainComponent,
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
