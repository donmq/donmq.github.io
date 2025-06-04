import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CompanyAddComponent } from './company-add/company-add.component';
import { CompanyEditComponent } from './company-edit/company-edit.component';
import { CompanyMainComponent } from './company-main/company-main.component';

const routes: Routes = [
  {
    path: '',
    component: CompanyMainComponent,
    data: {
      title: 'Company Main',
      breadcrumb: 'Company'
    }
  },
  {
    path: 'add',
    component: CompanyAddComponent,
    data: {
      title: 'Company Add',
      breadcrumb: 'CompanyAdd'
    }
  },
  {
    path: 'edit/:edit',
    component: CompanyEditComponent,
    data: {
      title: 'Company Edit',
      breadcrumb: 'CompanyEdit'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CompanyManageRoutingModule { }
