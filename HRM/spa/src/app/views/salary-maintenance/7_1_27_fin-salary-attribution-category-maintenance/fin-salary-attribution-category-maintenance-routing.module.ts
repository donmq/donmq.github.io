import { formGuard } from '@guards/app.guard';


import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { FinSalaryAttributionCategoryMaintenanceResolver } from '@services/salary-maintenance/s_7_1_27_fin-salary-attribution-category-maintenance.service';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {

      dataResolved: FinSalaryAttributionCategoryMaintenanceResolver
    },
    data: {
      title: 'Main'
    }
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: { dataResolved: FinSalaryAttributionCategoryMaintenanceResolver },
    data: {
      title: 'Add'
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FinSalaryAttributionCategoryMaintenanceRoutingModule { }
