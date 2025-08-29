import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ModalComponent } from './modal/modal.component';
import { MainComponent } from './main/main.component';

import { formGuard } from '@guards/app.guard';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,

  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: ModalComponent,
    data: {
      title: 'Add'
    }
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: ModalComponent,
    data: {
      title: 'Edit'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContractManagementRoutingModule { }
