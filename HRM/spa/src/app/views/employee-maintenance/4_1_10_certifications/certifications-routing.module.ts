
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { certificationsResolver } from '@services/employee-maintenance/s_4_1_10_certifications.service';
import { formGuard } from '@guards/app.guard';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {
      dataResolved: certificationsResolver
    },
    data: {
      title: 'Main'
    }
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: { dataResolved: certificationsResolver },
    data: {
      title: 'Add'
    },
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: { dataResolved: certificationsResolver },
    data: {
      title: 'Edit'
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CertificationsRoutingModule { }
