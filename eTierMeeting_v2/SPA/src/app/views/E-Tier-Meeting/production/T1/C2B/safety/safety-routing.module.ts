import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductionT1C2BSafetyResolver } from '../../../../../../_core/_resolvers/production/T1/C2B/production-t1-c2b-safety.resolver';

import { SafetymainComponent } from './safetymain/safetymain.component';

const routes: Routes = [
  {
    path: 'safetymain/CTB/:deptId',
    component: SafetymainComponent,
    resolve: { res: ProductionT1C2BSafetyResolver },
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
