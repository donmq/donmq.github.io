
import { formGuard } from '@guards/app.guard';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {

    },
  },
  {
    path: 'query',
    canMatch: [formGuard],
    component: FormComponent,
    data: {
      title: 'Query'
    }
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: FormComponent,
    data: {
      title: 'Add'
    }
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: FormComponent,
    data: {
      title: 'Edit'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LoanedMonthlyAttendanceDataMaintenanceRoutingModule { }
