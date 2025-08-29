import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';

import { formGuard } from '@guards/app.guard';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,

  },
  {
    path: 'query',
    canMatch: [formGuard],
    component: FormComponent,
    data: {
      title: 'Query'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MonthlySalaryMasterFileBackupQueryRoutingModule { }
