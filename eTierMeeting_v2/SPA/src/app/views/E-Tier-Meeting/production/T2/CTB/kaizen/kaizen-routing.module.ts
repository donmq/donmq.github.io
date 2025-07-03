import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { KaizenmainComponent } from './kaizenmain/kaizenmain.component';

const routes: Routes = [
  {
    path: 'kaizenmain/CTB/:deptId',
    component: KaizenmainComponent,
    data: {
      title: 'Kaizen-MainPage',
      page_Name: 'Kaizen'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class KaizenRoutingModule { }
