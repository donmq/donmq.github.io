import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { formGuard } from '@guards/app.guard';
import { MainComponent } from './main/main.component';
import { FormAddComponent } from './form-add/form-add.component';
import { FormEditComponent } from './form-edit/form-edit.component';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: FormEditComponent,
    data: {
      title: 'Edit'
    }
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: FormAddComponent,
    data: {
      title: 'Add'
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RewardAndPenaltyReasonCodeMaintenanceRoutingModule { }
