import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BuildingAddComponent } from './building-add/building-add.component';
import { BuildingEditComponent } from './building-edit/building-edit.component';
import { BuildingMainComponent } from './building-main/building-main.component';

const routes: Routes = [
  {
    path: '',
    component: BuildingMainComponent,
    data: {
      breadcrumb: 'BuildingMain'
    }
  },
  {
    path: 'add',
    component: BuildingAddComponent,
    data: {
      breadcrumb: 'BuildingAdd'
    }
  },
  {
    path: 'edit',
    component: BuildingEditComponent,
    data: {
      breadcrumb: 'BuildingEdit'
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BuildingManageRoutingModule { }
