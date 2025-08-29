import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { formGuard } from '@guards/app.guard';
import { shiftManagementProgramResolver } from '@services/attendance-maintenance/s_5_1_10_shift-management-program.service';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {
      dataResolved: shiftManagementProgramResolver
    },
    data: {
      title: 'Main'
    }
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: { dataResolved: shiftManagementProgramResolver },
    data: {
      title: 'Add'
    },
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: { dataResolved: shiftManagementProgramResolver },
    data: {
      title: 'Edit'
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ShiftManagementProgramRoutingModule { }
