import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { AddComponent } from './add/add.component';
import { EditComponent } from './edit/edit.component';

import { formGuard } from '@guards/app.guard';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: AddComponent,
    data: {
      title: 'Add'
    }
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: EditComponent,
    data: {
      title: 'Edit'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeTransferHistoryRoutingModule { }
