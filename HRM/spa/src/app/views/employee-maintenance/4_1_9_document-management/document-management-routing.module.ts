import { formGuard } from '@guards/app.guard';


import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { documentManagementResolver } from '@services/employee-maintenance/s_4_1_9_document-management.service';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {
      dataResolved: documentManagementResolver
    },
    data: {
      title: 'Main'
    }
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: { dataResolved: documentManagementResolver },
    data: {
      title: 'Add'
    },
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: { dataResolved: documentManagementResolver },
    data: {
      title: 'Edit'
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DocumentManagementRoutingModule { }
