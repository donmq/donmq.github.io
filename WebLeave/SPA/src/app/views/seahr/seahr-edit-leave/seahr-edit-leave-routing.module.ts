import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SeahrEditLeaveComponent } from './seahr-edit-leave-main/seahr-edit-leave.component';

const routes: Routes = [
  {
    path: '',
    component: SeahrEditLeaveComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SeahrEditLeaveRoutingModule { }
