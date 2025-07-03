import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductionT1UPFModelPreparationResolver } from '../../../../../../_core/_resolvers/production/T1/UPF/production-t1-upf-model-preparation.resolver';
import { ModelpreparationMainComponent } from './modelpreparationmain/modelpreparationmain.component';

const routes: Routes = [{
  path: 'modelpreparationmain/UPF/:deptId',
  component: ModelpreparationMainComponent,
  resolve: { res: ProductionT1UPFModelPreparationResolver },
  data: {
    title: 'ModelPreparation-MainPage',
    page_Name: 'Model Preparation'
  }
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ModelPreparationRoutingModule { }
