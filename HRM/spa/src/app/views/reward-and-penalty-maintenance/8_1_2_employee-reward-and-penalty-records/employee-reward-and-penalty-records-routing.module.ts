import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { employeeRewardAndPenaltyResolver } from '@services/reward-and-penalty-maintenance/s_8_1_2_employee-reward-and-penalty-records.service';
import { formGuard } from '@guards/app.guard';
import { FormComponent } from './form/form.component';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {
      dataResolved: employeeRewardAndPenaltyResolver
    },
    data: {
      title: 'Main'
    }
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: FormComponent,
    data: {
      title: 'Add'
    },
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: FormComponent,
    data: {
      title: 'Edit'
    },
  },
  {
    path: 'query',
    canMatch: [formGuard],
    component: FormComponent,
    data: {
      title: 'Query'
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeRewardAndPenaltyRecordsRoutingModule { }
