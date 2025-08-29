import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { resolverDivisions, resolverWorkShiftTypes } from '@services/attendance-maintenance/s_5_1_2_shift-schedule-setting.service';



import { formGuard } from '@guards/app.guard';
import { FormComponent } from './form/form.component';
import { MainComponent } from './main/main.component';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {

      resolverDivisions: resolverDivisions,
      resolverWorkShiftTypes: resolverWorkShiftTypes,
    }
  },
  {
    path: 'add',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: {
      resolverDivisions: resolverDivisions,
      resolverWorkShiftTypes: resolverWorkShiftTypes,
    },
    data: {
      title: 'Add',
    }
  },
  {
    path: 'edit',
    canMatch: [formGuard],
    component: FormComponent,
    resolve: {
      resolverDivisions: resolverDivisions,
      resolverWorkShiftTypes: resolverWorkShiftTypes,
    },
    data: {
      title: 'Edit',
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ShiftScheduleSettingRoutingModule { }
