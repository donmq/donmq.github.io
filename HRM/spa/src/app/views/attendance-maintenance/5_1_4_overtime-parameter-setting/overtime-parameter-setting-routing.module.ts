import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { EditComponent } from './edit/edit.component';
import { AddComponent } from './add/add.component';

import { formGuard } from '../../../_core/guards/app.guard';
const routes: Routes = [
  {
    path: '',
    component: MainComponent,
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: EditComponent,
    data: {
      title: 'Edit',
    },
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: AddComponent,
    data: {
      title: 'Add',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OvertimeParameterSettingRoutingModule { }
