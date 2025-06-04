import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoryManagementViewComponent } from './category-management-view/category-management-view.component';

const routes: Routes = [
  {
    path: '',
    component: CategoryManagementViewComponent,
    data: {
      breadcrumb: 'Category'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CategoryManageRoutingModule { }
