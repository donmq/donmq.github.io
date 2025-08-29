import { formGuard } from '@guards/app.guard';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { FormtypecodeComponent } from './formtypecode/formtypecode.component';


const routes: Routes = [
  {
    path: '',
    component: MainComponent,

  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: FormtypecodeComponent,
    data: {
      title: 'Add',
    },
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: FormtypecodeComponent,
    data: {
      title: 'Edit',
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CodeTypeMaintenanceRoutingModule { }
