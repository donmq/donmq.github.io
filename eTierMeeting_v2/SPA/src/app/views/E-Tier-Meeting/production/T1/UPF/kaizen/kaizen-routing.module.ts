import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductionT1UPFKaizenResolver } from '../../../../../../_core/_resolvers/production/T1/UPF/production-t1-upf-kaizen.resolver';
import { KaizenmainComponent } from './kaizenmain/kaizenmain.component';

const routes: Routes = [{
  path: 'kaizenmain/UPF/:deptId',
  component: KaizenmainComponent,
  resolve: { res: ProductionT1UPFKaizenResolver },
  data: {
    title: 'Kaizen-MainPage',
    page_Name: 'Kaizen'
  }
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class KaizenRoutingModule { }
