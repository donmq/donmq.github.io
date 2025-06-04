import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AreaAddComponent } from './area-add/area-add.component';
import { AreaEditComponent } from './area-edit/area-edit.component';
import { AreaMainComponent } from './area-main/area-main.component';

const routes: Routes = [
  {
    path: '',
    component: AreaMainComponent,
    data: {
      breadcrumb: 'AreaMain'
    }
  },
  {
    path: 'add',
    component: AreaAddComponent,
    data: {
      breadcrumb: 'AreaAdd'
    }
  },
  {
    path: 'edit',
    component: AreaEditComponent,
    data: {
      breadcrumb: 'AreaEdit'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AreaManageRoutingModule { }
