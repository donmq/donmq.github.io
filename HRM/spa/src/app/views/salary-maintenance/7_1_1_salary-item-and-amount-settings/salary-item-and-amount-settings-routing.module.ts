import { formGuard } from '@guards/app.guard';


import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { salaryItemAndAmountSettingsResolver } from '@services/salary-maintenance/s_7_1_1_salary-item-and-amount-settings.service';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {

      dataResolved: salaryItemAndAmountSettingsResolver
    },
    data: {
      title: 'Main'
    }
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: { dataResolved: salaryItemAndAmountSettingsResolver },
    data: {
      title: 'Add'
    },
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: { dataResolved: salaryItemAndAmountSettingsResolver },
    data: {
      title: 'Edit'
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SalaryItemandAmountSettingsRoutingModule { }
