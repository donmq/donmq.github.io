import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SeahrConfirmComponent } from './seahr-confirm/seahr-confirm.component';

const routes: Routes = [
  {
    path: '',
    component: SeahrConfirmComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SeahrConfirmRoutingModule { }
