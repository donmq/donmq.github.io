import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from '../6_1_3_apply_social_insurance_benefits_maintenance/main/main.component';
import { formGuard } from '@guards/app.guard';

import { FormComponent } from '../6_1_3_apply_social_insurance_benefits_maintenance/form/form.component';

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
      title: 'Edit'
    }
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: FormComponent,
    data: {
      title: 'Add'
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ApplySocialInsuranceBenefitsMaintenanceRoutingModule { }
