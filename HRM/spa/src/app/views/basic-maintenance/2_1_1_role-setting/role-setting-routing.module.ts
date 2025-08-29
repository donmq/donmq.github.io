import { dataResolver } from '@services/basic-maintenance/s_2_1_1_role-setting.service';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';

import { formGuard } from '@guards/app.guard';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {

      dataResolved: dataResolver
    },
    data: {
      title: 'Main'
    }
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: { dataResolved: dataResolver },
    data: {
      title: 'Add',
    },
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: { dataResolved: dataResolver },
    data: {
      title: 'Edit',
    },
  },
  {
    path: 'copy',
    component: FormComponent,
    resolve: { dataResolved: dataResolver },
    data: {
      title: 'Copy',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RoleSettingRoutingModule { }
