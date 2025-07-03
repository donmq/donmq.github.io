import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ModelPreparationmainComponent } from './modelpreparationmain/modelpreparationmain.component';

const routes: Routes = [
  {
    path: 'modelpreparationmain/STF/:deptId',
    component: ModelPreparationmainComponent,
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
