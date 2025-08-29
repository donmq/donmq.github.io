import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MainPageComponent411 } from './main-page/main-page.component';
import { MainComponent411 } from './main/main.component';
import { UploadFormComponent411 } from './upload-form/upload-form.component';
import { formGuard } from '@guards/app.guard';

const routes: Routes = [
  {
    path: '',
    component: MainComponent411,
    data: {
      title: 'Main'
    },
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: MainPageComponent411,
    data: {
      title: 'Add'
    },
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: MainPageComponent411,
    data: {
      title: 'Edit'
    },
  },
  {
    path: 'rehire',
    canMatch: [formGuard],
    component: MainPageComponent411,
    data: {
      title: 'Rehire'
    },
  },
  {
    path: 'query',
    canMatch: [formGuard],
    component: MainPageComponent411,
    data: {
      title: 'Query'
    },
  },
  {
    path: 'upload',
    canMatch: [formGuard],
    component: UploadFormComponent411,
    data: {
      title: 'Upload'
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeBasicInformationMaintenanceRoutingModule { }
