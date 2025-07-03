import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductionT1C2BModelPreparationResolver } from '../../../../../../_core/_resolvers/production/T1/C2B/production-t1-c2b-model-preparation.resolver';
import { ModelPreparationmainComponent } from './modelpreparationmain/modelpreparationmain.component';

const routes: Routes = [
  {
    path: 'modelpreparationmain/CTB/:deptId',
    component: ModelPreparationmainComponent,
    resolve: { res: ProductionT1C2BModelPreparationResolver },
    data: {
      title: 'ModelPreparation-MainPage',
      page_Name: 'Model Preparation'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ModelPreparationRoutingModule { }
