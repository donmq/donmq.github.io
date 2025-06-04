import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeDetailComponent } from './employee-detail/employee-detail.component';
import { EmployeeEditComponent } from './employee-edit/employee-edit.component';
import { EmployeeListComponent } from './employee-list/employee-list.component';

const routes: Routes = [
  {
    path: "",
    component: EmployeeListComponent,
    data: {
      breadcrumb: 'Employee',
      title: "Employee"
    },
  },
  {
    path: "edit/:id",
    component: EmployeeEditComponent,
    data: {
      breadcrumb: 'Employee',
      title: "Edit Employee"
    },
  },
  {
    path: "detail/:id/:key",
    component: EmployeeDetailComponent,
    data: {
      breadcrumb: 'EmployeeDetail',
      title: "Detail Employee"
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeManageRoutingModule { }
