import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { resolverTypeSeqs } from '@services/basic-maintenance/s_2_1_4-code-maintenance.service';

import { FormComponent } from './form/form.component';
import { MainComponent } from './main/main.component';
import { formGuard } from '@guards/app.guard';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {

      resolverTypeSeqs: resolverTypeSeqs
    }
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: { resolverTypeSeqs: resolverTypeSeqs },
    data: {
      title: 'Add',
      form_title: 'BasicMaintenance.CodeMaintain.CreateNew'
    }
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: { resolverTypeSeqs: resolverTypeSeqs },
    data: {
      title: 'Edit',
      form_title: 'BasicMaintenance.CodeMaintain.Edit'
    }
  },
  {
    path: 'query',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: { resolverTypeSeqs: resolverTypeSeqs },
    data: {
      title: 'Query',
      form_title: 'BasicMaintenance.CodeMaintain.Query'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CodeMaintenanceRoutingModule { }
