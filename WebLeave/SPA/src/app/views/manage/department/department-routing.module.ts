import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DepartmentAddComponent } from './department-add/department-add.component';
import { DepartmentMainComponent } from './department-main/department-main.component';
import { DepartmentUpdateComponent } from './department-update/department-update.component';

const routes: Routes = [
  {
    path: '',
    component: DepartmentMainComponent
  },

  {
    path: 'add',
    component: DepartmentAddComponent,
  },
  {
    path: 'update',
    component: DepartmentUpdateComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DepartmentRoutingModule {}
