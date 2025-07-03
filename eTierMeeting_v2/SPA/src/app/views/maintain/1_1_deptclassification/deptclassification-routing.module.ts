import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DeptclassificationListComponent } from "./deptclassification-list/deptclassification-list.component";
import { DeptclassificationAddComponent } from "./deptclassification-add/deptclassification-add.component";
import { DeptclassificationEditComponent } from "./deptclassification-edit/deptclassification-edit.component";

const routes: Routes = [
  {
    path: '',
    component: DeptclassificationListComponent,
    data: {
      title: 'Classification'
    }
  },
  {
    path: "add",
    component: DeptclassificationAddComponent,
    data: {
      title: "Add Classification",
    },
  },
  {
    path: "edit",
    component: DeptclassificationEditComponent,
    data: {
      title: "Edit Classification",
    },
  },
  // {
  //   path: '',
  //   data: {
  //     title: ''
  //   },
  //   children: [
  //     {
  //       path: '',
  //       redirectTo: 'classification'
  //     },
  //     {
  //       path: 'classification',
  //       component: DeptclassificationListComponent,
  //       data: {
  //         title: 'Classification'
  //       }
  //     },
  //     {
  //       path: "add",
  //       component: DeptclassificationAddComponent,
  //       data: {
  //         title: "Add Classification",
  //       },
  //     },
  //     {
  //       path: "edit",
  //       component: DeptclassificationAddComponent,
  //       data: {
  //         title: "Edit Classification",
  //       },
  //     },
  //   ]
  // }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DeptclassificationRoutingModule { }
