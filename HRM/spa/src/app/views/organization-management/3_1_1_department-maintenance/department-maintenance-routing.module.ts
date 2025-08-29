import { formGuard } from '@guards/app.guard';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';


const routes: Routes = [
  {
    path: '',
    component: MainComponent,

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
export class DepartmentMaintenanceRoutingModule { }
