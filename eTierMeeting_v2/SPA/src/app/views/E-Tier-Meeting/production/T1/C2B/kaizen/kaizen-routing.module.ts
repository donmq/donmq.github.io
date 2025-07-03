import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductionT1C2BKaizenResolver } from '../../../../../../_core/_resolvers/production/T1/C2B/production-t1-c2b-kaizen.resolver';

import { KaizenmainComponent } from './kaizenmain/kaizenmain.component';

const routes: Routes = [
  {
    path: 'kaizenmain/CTB/:deptId',
    component: KaizenmainComponent,
    resolve: { res: ProductionT1C2BKaizenResolver },
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
