import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PositionAddComponent } from './position-add/position-add.component';
import { PositionEditComponent } from './position-edit/position-edit.component';
import { PositionMainComponent } from './position-main/position-main.component';

const routes: Routes = [
  {
    path: '',
    component: PositionMainComponent,
    data: {
      title: 'Position Main',
      breadcrumb: 'Position'
    }
  },
  {
    path: 'add',
    component: PositionAddComponent,
    data: {
      title: 'Position Add',
      breadcrumb: 'PositionAdd'
    }
  },
  {
    path: 'edit/:edit',
    component: PositionEditComponent,
    data: {
      title: 'Position Edit',
      breadcrumb: 'PositionEdit'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PositionManageRoutingModule { }
