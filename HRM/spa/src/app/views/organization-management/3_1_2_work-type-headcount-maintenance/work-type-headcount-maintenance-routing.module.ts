import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { resolverDivisions, resolverFactories } from '@services/organization-management/s_3_1_2_work-type-headcount-maintenance.service';

import { FormComponent } from './form/form.component';
import { MainComponent } from './main/main.component';
import { formGuard } from '@guards/app.guard';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {

      resolverDivisions: resolverDivisions,
      resolverFactories: resolverFactories,
    }
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: {
      resolverDivisions: resolverDivisions,
      resolverFactories: resolverFactories,
    },
    data: {
      title: 'Add',
      form_title: 'OrganizationManagement.WorkTypeHeadcountMaintenance.CreateNew'
    }
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: {
      resolverDivisions: resolverDivisions,
      resolverFactories: resolverFactories,
    },
    data: {
      title: 'Edit',
      form_title: 'OrganizationManagement.WorkTypeHeadcountMaintenance.Edit'
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WorkTypeHeadcountMaintenanceRoutingModule { }
