import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { formGuard } from '@guards/app.guard';
import { FormComponent } from './form/form.component';
import { title } from 'process';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: FormComponent,
    data: {
      title: "Edit"
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FinSalaryCloseMaintenanceRoutingModule { }
